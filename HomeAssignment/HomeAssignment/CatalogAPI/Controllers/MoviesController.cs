using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Newtonsoft.Json.Linq;
using RestSharp;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _service.GetAsync();
        }

        // GET: api/Movies/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            var movie = await _service.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> PutMovie(string id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            bool movieExists = await MovieExists(id);

            if (movieExists)
            {
                await _service.UpdateAsync(id, movie);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            await _service.CreateAsync(movie);

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var movie = await _service.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _service.RemoveAsync(id);

            return NoContent();
        }

        private async Task<bool> MovieExists(string id)
        {
            var movie = await _service.GetAsync(id);
            return movie != null;
        }

        [HttpGet("titles")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genre)
        {
            try
            {
                var movies = await _service.GetMoviesByGenreAsync(genre);
                if (movies == null || movies.Count == 0)
                {
                    return NotFound();
                }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
