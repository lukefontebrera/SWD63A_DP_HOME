using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WishlistAPI.Models;
using WishlistAPI.Services;

namespace WishlistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishedMoviesController : ControllerBase
    {
        private readonly WishService _service;

        public WishedMoviesController(WishService service)
        {
            _service = service;
        }

        // GET: api/WishedMovies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishedMovie>>> GetWishedMovies()
        {
            return await _service.GetAsync();
        }

        // GET: api/WishedMovies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WishedMovie>> GetWishedMovie(string id)
        {
            var wishedItem = await _service.GetAsync(id);

            if (wishedItem == null)
            {
                return NotFound();
            }

            return wishedItem;
        }

        // PUT: api/WishedMovies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishedMovie(string id, WishedMovie wishedMovie)
        {
            if (id != wishedMovie.Id)
            {
                return BadRequest();
            }

            bool wishedItemExists = await WishedMovieExists(id);

            if (wishedItemExists)
            {
                await _service.UpdateAsync(id, wishedMovie);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/WishedMovies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WishedMovie>> PostWishedMovie(WishedMovie wishedMovie)
        {
            string movieId = wishedMovie.MovieId;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Movies/titles/movies/search/title/{wishedMovie.Title}");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Title details: {content}");

                        var jsonContent = JArray.Parse(content);

                        if (jsonContent == null || !jsonContent.Any())
                        {
                            response = await httpClient.GetAsync($"http://localhost:5003/gateway/Movies/titles/tv/search/title/{wishedMovie.Title}");
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

                        wishedMovie.PictureUri = firstItem["pictureUri"].Value<string>();
                        wishedMovie.Title = firstItem["title"].Value<string>();
                        wishedMovie.MovieId = movieId;

                        await _service.CreateAsync(wishedMovie);

                        return Ok();
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

        // DELETE: api/WishedMovies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishedMovie(string id)
        {
            var wishedItem = await _service.GetAsync(id);
            if (wishedItem == null)
            {
                return NotFound();
            }

            await _service.RemoveAsync(id);

            return NoContent();
        }

        private async Task<bool> WishedMovieExists(string id)
        {
            var wishedItem = await _service.GetAsync(id);
            return wishedItem != null;
        }
    }
}
