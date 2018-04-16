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
        public IEnumerable<FilmSerializer> GetFilms()
        {
            return _context.Films
               .Include(f => f.Country)
               .Include(f => f.Genre)
               .Include(f => f.Director)
               .Include(f => f.AgeLimit)
               .ToList()
               .Select(f => new FilmSerializer
               {
                   LINK = f.LINK,
                   Name = f.Name,
                   Poster = f.Poster,
                   ReleaseYear = f.ReleaseYear,
                   Country = f.Country.Name,
                   Genre = f.Genre.Name,
                   Director = f.Director.Name + " " + f.Director.Surname,
                   Duration = f.Duration,
                   AgeLimit = f.AgeLimit.Value,
                   Archived = f.Archived,
                   Description = f.Description,
                   TrailerLink = f.TrailerLink,
                   GlobalRating = f.GlobalRating,
                   LocalRating = f.LocalRating,
               })
               .AsQueryable();
        }
        [HttpGet("featured")]
        public IEnumerable<FilmSerializer> GetFeaturedFilms()
        {
            return _context.Films
                  .Include(f => f.Country)
                  .Include(f => f.Genre)
                  .Include(f => f.Director)
                  .Include(f => f.AgeLimit)
                  .OrderByDescending(f => _context.Seats
                                            .Include(seat => seat.Session) 
                                            .Where(seat => seat.Session.FilmLINK == f.LINK).Count()
                   )
                  .Take(4)
                  .ToList()
                  .Select(f => new FilmSerializer
                  {
                      LINK = f.LINK,
                      Name = f.Name,
                      Poster = f.Poster,
                      ReleaseYear = f.ReleaseYear,
                      Country = f.Country.Name,
                      Genre = f.Genre.Name,
                      Director = f.Director.Name + " " + f.Director.Surname,
                      Duration = f.Duration,
                      AgeLimit = f.AgeLimit.Value,
                      Archived = f.Archived,
                      Description = f.Description,
                      TrailerLink = f.TrailerLink,
                      GlobalRating = f.GlobalRating,
                      LocalRating = f.LocalRating,
                  })
                  .AsQueryable();
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

        public class FilmSerializer
        {
            public int LINK { get; set; }
            public string Name { get; set; }
            public string Poster { get; set; }
            public int ReleaseYear { get; set; }
            public string Country { get; set; }
            public string Genre { get; set; }
            public string Director { get; set; }
            public string Duration { get; set; }
            public string AgeLimit { get; set; }
            public bool? Archived { get; set; }
            public string Description { get; set; }
            public string TrailerLink { get; set; } 
            public int? GlobalRating { get; set; }
            public int? LocalRating { get; set; }
        }
    }
}