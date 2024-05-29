using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishlistAPI.Models;

namespace WishlistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishedMoviesController : ControllerBase
    {
        private readonly WishListContext _context;

        public WishedMoviesController(WishListContext context)
        {
            _context = context;
        }

        // GET: api/WishedMovies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishedMovie>>> GetWishedMovies()
        {
          if (_context.WishedMovies == null)
          {
              return NotFound();
          }
            return await _context.WishedMovies.ToListAsync();
        }

        // GET: api/WishedMovies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WishedMovie>> GetWishedMovie(string id)
        {
          if (_context.WishedMovies == null)
          {
              return NotFound();
          }
            var wishedMovie = await _context.WishedMovies.FindAsync(id);

            if (wishedMovie == null)
            {
                return NotFound();
            }

            return wishedMovie;
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

            _context.Entry(wishedMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishedMovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WishedMovies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WishedMovie>> PostWishedMovie(WishedMovie wishedMovie)
        {
          if (_context.WishedMovies == null)
          {
              return Problem("Entity set 'WishListContext.WishedMovies'  is null.");
          }
            _context.WishedMovies.Add(wishedMovie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WishedMovieExists(wishedMovie.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWishedMovie", new { id = wishedMovie.Id }, wishedMovie);
        }

        // DELETE: api/WishedMovies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishedMovie(string id)
        {
            if (_context.WishedMovies == null)
            {
                return NotFound();
            }
            var wishedMovie = await _context.WishedMovies.FindAsync(id);
            if (wishedMovie == null)
            {
                return NotFound();
            }

            _context.WishedMovies.Remove(wishedMovie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishedMovieExists(string id)
        {
            return (_context.WishedMovies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
