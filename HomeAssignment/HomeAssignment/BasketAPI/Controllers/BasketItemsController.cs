using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BasketAPI.Services;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly BasketService _service;

        public BasketItemsController(BasketService service)
        {
            _service = service;
        }

        // GET: api/BasketItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketItem>>> GetBasketItems()
        {
            return await _service.GetAsync();
        }

        // GET: api/BasketItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketItem>> GetBasketItem(string id)
        {
            var basket = await _service.GetAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return basket;
        }

        // PUT: api/BasketItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketItem(string id, BasketItem basketItem)
        {
            if (id != basketItem.Id)
            {
                return BadRequest();
            }

            bool basketItemExists = await BasketItemExists(id);

            if (basketItemExists)
            {
                await _service.UpdateAsync(id, basketItem);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/BasketItems
        [HttpPost]
        public async Task<ActionResult<BasketItem>> PostBasketItem(BasketItem basketItem)
        {
            string movieId = basketItem.MovieId;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"http://localhost:5032/api/Movies/{movieId}");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Movie details: {content}");

                        var jsonContent = JsonConvert.DeserializeObject<JToken>(content);

                        if (jsonContent == null)
                        {
                            Console.WriteLine("Movie content is null.");
                            return NotFound("Movie content is null.");
                        }

                        var stockQuantity = jsonContent["stock"].Value<decimal>();

                        if (basketItem.Quantity <= stockQuantity)
                        {
                            basketItem.MovieName = jsonContent["title"].Value<string>();
                            basketItem.UnitPrice = jsonContent["price"].Value<decimal>();
                            basketItem.MovieId = movieId;

                            await _service.CreateAsync(basketItem);

                            return CreatedAtAction("GetBasketItem", new { id = basketItem.Id }, basketItem);
                        }
                        else
                        {
                            Console.WriteLine("Insufficient stock quantity.");
                            return BadRequest("Insufficient stock quantity.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Movie not found: {response.ReasonPhrase}");
                        return NotFound("Movie not found.");
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

        private async Task<bool> BasketItemExists(string id)
        {
            var basket = await _service.GetAsync(id);
            return basket != null;
        }
    }
}
