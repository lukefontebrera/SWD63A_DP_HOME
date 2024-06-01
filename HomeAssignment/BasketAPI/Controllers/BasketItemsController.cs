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
using Publisher.Services;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly BasketService _service;
        private readonly PublisherService _publisherService;

        public BasketItemsController(BasketService service, PublisherService publisherService)
        {
            _service = service;
            _publisherService = publisherService;
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
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Movies/titles/movies/search/title/{basketItem.Title}");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Title details: {content}");

                        var jsonContent = JArray.Parse(content); // Parse the response as a JArray

                        if (jsonContent == null || !jsonContent.Any())
                        {
                            response = await httpClient.GetAsync($"http://localhost:5003/gateway/Movies/titles/tv/search/title/{basketItem.Title}");
                            content = await response.Content.ReadAsStringAsync();
                            jsonContent = JArray.Parse(content);
                        }

                        if (jsonContent == null || !jsonContent.Any())
                        {
                            Console.WriteLine("Title content is null or empty.");
                            return NotFound("Title content is null or empty.");
                        }

                        var firstItem = jsonContent.First;

                        if (firstItem == null)
                        {
                            Console.WriteLine("No items found in the response.");
                            return NotFound("No items found in the response.");
                        }

                        basketItem.PictureUri = firstItem["pictureUri"].Value<string>();
                        basketItem.Title = firstItem["title"].Value<string>();
                        basketItem.UnitPrice = firstItem["price"].Value<decimal>();
                        basketItem.MovieId = movieId;

                        string message = "Item added to the basket: " + JsonConvert.SerializeObject(basketItem);
                        await _publisherService.PublishMessage(message, "BasketAPI");

                        await _service.CreateAsync(basketItem);

                        return CreatedAtAction("GetBasketItem", new { id = basketItem.Id }, basketItem);
                    }
                    else
                    {
                        Console.WriteLine($"Title not found: {response.ReasonPhrase}");
                        return NotFound("Title not found.");
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

        // DELETE: api/BasketItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem(string id)
        {
            var basketItem = await _service.GetAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }

            await _service.RemoveAsync(id);

            return NoContent();
        }
    }
}
