using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KINOv2.Data;
using KINOv2.Models.MainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<Film> GetFavorite()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return null;
            }
            return _context.Films.Where(f => f.FilmUsers.Where(fu => fu.ApplicationUserId == user.Id).Count() > 0);
        }
        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.LINK == id);
        }

    }
}