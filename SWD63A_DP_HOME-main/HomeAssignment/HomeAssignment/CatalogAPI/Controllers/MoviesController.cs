using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _service;

        public MoviesController(MovieService service)
        {
            _service = service;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            var movies = _service.GetMoviesByGenre(genre);
            return Ok(movies);
        }

        // GET: api/Movies/{name}
        [HttpGet("title/{name}")]
        public ActionResult<IEnumerable<Movie>> GetMovieByName(string name)
        {
            var movie = _service.GetSpecificMovie(name);
            return Ok(movie);
        }

    }
}
