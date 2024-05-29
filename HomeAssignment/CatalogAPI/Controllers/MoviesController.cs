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
    }
}
