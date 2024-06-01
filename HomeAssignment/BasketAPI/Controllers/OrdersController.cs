using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketAPI.Models;
using BasketAPI.Services;
using System.Text;
using Newtonsoft.Json;
using WishlistAPI.Models;
using Publisher.Services;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        private readonly PublisherService _publisherService;

        public OrdersController(OrderService service, PublisherService publisherService)
        {
            _service = service;
            _publisherService = publisherService;

        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _service.GetAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _service.GetAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Orders");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        order.Id = GenerateId();
                        await _service.CreateAsync(order);

                        string message = "Order has been completed: " + JsonConvert.SerializeObject(order);
                        await _publisherService.PublishMessage(message, "BasketAPI");

                        var basketItemsResponse = await httpClient.GetAsync($"http://localhost:5003/gateway/BasketItems");
                        if (basketItemsResponse.IsSuccessStatusCode)
                        {
                            var content = await basketItemsResponse.Content.ReadAsStringAsync();
                            var basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(content);

                            var currentUserBasketItems = basketItems.Where(item => item.User == order.User);

                            foreach (var basketItem in currentUserBasketItems)
                            {
                                var deleteResponse = await httpClient.DeleteAsync($"http://localhost:5003/gateway/BasketItems/{basketItem.Id}");
                                if (!deleteResponse.IsSuccessStatusCode)
                                {
                                    Console.WriteLine($"Failed to delete basket item {basketItem.Id}: {deleteResponse.ReasonPhrase}");
                                }
                            }

                            message = "User notified about new order: " + JsonConvert.SerializeObject(order);
                            await _publisherService.PublishMessage(message, "BasketAPI");

                            var wishedMoviesResponse = await httpClient.GetAsync($"http://localhost:5003/gateway/WishedMovies");
                            if (wishedMoviesResponse.IsSuccessStatusCode)
                            {
                                var wishedMoviesContent = await wishedMoviesResponse.Content.ReadAsStringAsync();
                                var wishedMovies = JsonConvert.DeserializeObject<List<WishedMovie>>(wishedMoviesContent);

                                foreach (var movieId in order.Movies.Select(movie => movie.MovieId))
                                {
                                    var movieToRemove = wishedMovies.FirstOrDefault(movie => movie.MovieId == movieId);
                                    if (movieToRemove != null)
                                    {
                                        var deleteResponse = await httpClient.DeleteAsync($"http://localhost:5003/gateway/WishedMovies/{movieToRemove.Id}");
                                        if (!deleteResponse.IsSuccessStatusCode)
                                        {
                                            Console.WriteLine($"Failed to delete basket item {movieToRemove.Id}: {deleteResponse.ReasonPhrase}");
                                        }
                                    }
                                }
                            }

                            else
                            {
                                // Handle unsuccessful response
                                Console.WriteLine($"Failed to fetch wished movies: {wishedMoviesResponse.ReasonPhrase}");
                            }
                        }
                        else
                        {
                            // Handle unsuccessful response
                            Console.WriteLine($"Failed to fetch basket items: {basketItemsResponse.ReasonPhrase}");
                        }

                        return Ok();
                    }
                    else
                    {
                        Console.WriteLine($"Order not found: {response.ReasonPhrase}");
                        return NotFound("Order not found.");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP Request error: {e.Message}");
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Error fetching movie data.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
                }
            }
        }




        private async Task<bool> OrderExists(string id)
        {
            var order = await _service.GetAsync(id);
            return order != null;
        }

        private string GenerateId()
        {
            Random random = new Random();
            StringBuilder hexString = new StringBuilder(24);

            for (int i = 0; i < 24; i++)
            {
                int randomNumber = random.Next(16);
                hexString.Append(randomNumber.ToString("X"));
            }

            return hexString.ToString();
        }
    }
}
