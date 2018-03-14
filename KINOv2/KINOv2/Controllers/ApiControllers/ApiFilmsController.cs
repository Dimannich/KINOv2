using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KINOv2.Data;
using KINOv2.Models.MainModels;

namespace KINOv2.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/film")]
    public class ApiFilmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiFilmsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/film
        [HttpGet]
        public IEnumerable<Film> GetFilms()
        {
            return _context.Films;
        }

        // GET: api/film/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var film = await _context.Films.SingleOrDefaultAsync(m => m.LINK == id);

            if (film == null)
            {
                return NotFound();
            }

            return Ok(film);
        }
        [HttpGet("favorite")]
        public IEnumerable<Film> GetFavorite(string user)
        {
            return _context.Films.Where(f => f.FilmUsers.Where(fu => fu.ApplicationUserId == user).Count() > 0);
        }
        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.LINK == id);
        }
    }
}