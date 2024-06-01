using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Publisher.Services;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _service;
        private readonly UpcomingService _upcomingService;
        private readonly PublisherService _publisherService;

        public MoviesController(MovieService service, UpcomingService upcomingService, PublisherService publisherService)
        {
            _service = service;
            _upcomingService = upcomingService;
            _publisherService = publisherService;
        }

        // GET: api/Movies/titles/{genre}
        [HttpGet("titles/movies/{genre}")]
        public ActionResult<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            var movies = _service.GetMoviesByGenre(genre);
            return Ok(movies);
        }

        // GET: api/Movies/titles/search/title/{name}
        [HttpGet("titles/movies/search/title/{name}")]
        public ActionResult<IEnumerable<Movie>> GetMovieByName(string name)
        {
            var movie = _service.GetSpecificMovie(name);
            return Ok(movie);
        }

        // GET: api/Movies/titles/{genre}
        [HttpGet("titles/tv/{genre}")]
        public ActionResult<IEnumerable<Movie>> GetTVShowByGenre(string genre)
        {
            var shows = _service.GetTVShowByGenre(genre);
            return Ok(shows);
        }

        // GET: api/Movies/titles/search/title/{name}
        [HttpGet("titles/tv/search/title/{name}")]
        public ActionResult<IEnumerable<Movie>> GetSpecificTVShow(string name)
        {
            var show = _service.GetSpecificTVShow(name);
            return Ok(show);
        }

        // GET: api/Movies/titles/x/upcoming
        [HttpGet("titles/x/upcoming")]
        public ActionResult<IEnumerable<Movie>> GetUpcomingMovies()
        {
            var show = _service.GetUpcomingMovies();
            return Ok(show);
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetUpcomingItems()
        {
            return await _upcomingService.GetAsync();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> AddUpcomingMovie(Movie movie)
        {
            string movieId = movie.Id;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"http://localhost:5003/gateway/Movies/titles/x/upcoming");
                    Console.WriteLine($"Received response: {(int)response.StatusCode} - {response.ReasonPhrase}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Title details: {content}");

                        var jsonContent = JArray.Parse(content);

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

                        movie.Id = GenerateId();
                        movie.Title = firstItem["title"].Value<string>();
                        movie.Description = firstItem["description"].Value<string>();
                        movie.Caption = firstItem["caption"].Value<string>();
                        movie.Price = firstItem["price"].Value<decimal>();
                        movie.PictureUri = firstItem["pictureUri"].Value<string>();
                        movie.ReleaseYear = firstItem["releaseYear"].Value<int>();
                        movie.ReleaseDate = firstItem["releaseDate"].Value<DateTime>();

                        string message = "Notified user of upcoming movie: " + JsonConvert.SerializeObject(movie);
                        await _publisherService.PublishMessage(message, "CatalogAPI");

                        await _upcomingService.CreateAsync(movie);

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
