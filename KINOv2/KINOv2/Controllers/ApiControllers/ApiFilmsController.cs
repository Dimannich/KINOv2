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
using KINOv2.Models.AdditionalEFEntities;

namespace KINOv2.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/film")]
    public class ApiFilmsController : Controller
    {
        private readonly ApplicationDbContext _context;
        ApiCommentsController apiComments;

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
                   AmountComment = _context.Comments.Count(c => c.FilmLINK == f.LINK)
               })
               .AsQueryable();
        }
        [HttpGet("featured")]
        public IEnumerable<FilmSerializer> GetFeaturedFilms()
        {
            apiComments = new ApiCommentsController(_context);
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
                      AmountComment = apiComments.GetComments(f.LINK).Count(),
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
        public IEnumerable<FilmSerializer> GetFavorite()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return null;
            }
            return _context.Films
                .Include(f => f.Country)
                .Include(f => f.Genre)
                .Include(f => f.Director)
                .Include(f => f.AgeLimit)
                .Include(f => f.FilmUsers)
                .Where(f => f.FilmUsers.Any(favorite=> favorite.ApplicationUserId == user.Id))
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
                    AmountComment = _context.Comments.Count(c => c.FilmLINK == f.LINK)
                })
                .AsQueryable();
        }
        [HttpGet("{id}/favorite/add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult AddToFavorite([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            // проверяем, может он уже добавлен
            if (_context.Films.Any(f => f.FilmUsers.Where(fu => fu.ApplicationUserId == user.Id).Count() > 0 && f.LINK == id))
            {
                return Json(new Dictionary<string, bool> { { "success", true } });
            }
            var film = _context.Films.FirstOrDefault(f => f.LINK == id);
            if (film == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Фильм не существует"}
                });
            var newFavorite = new FilmUser();
            newFavorite.Film = film;
            newFavorite.ApplicationUser = user;
            user.FilmUsers.Add(newFavorite);
            _context.SaveChanges();
            return Json(new Dictionary<string, bool> { { "success", true } });
        }
        [HttpGet("{id}/favorite/remove")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult RemoveFromFavorite([FromRoute] int id)
        {
            var user = _context.Users.Include(u=>u.FilmUsers).FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            var film = _context.Films.FirstOrDefault(f => f.LINK == id);
            if (film == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Фильм не существует"}
                });
            // проверяем, добавлен ли он
            var favorite = user.FilmUsers.FirstOrDefault(f => f.FilmLINK == id);
            if (favorite != null)
            {
                user.FilmUsers.Remove(favorite);
                _context.SaveChanges();
                return Json(new Dictionary<string, bool> { { "success", true } });
            }
            return Json(new Dictionary<string, bool> { { "success", true } });
        }
        [HttpGet("{id}/favorite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult IsFavorite([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            // проверяем, может он уже добавлен
            if (_context.Films.Any(f => f.FilmUsers.Where(fu => fu.ApplicationUserId == user.Id).Count() > 0 && f.LINK == id))
            {
                return Json(new Dictionary<string, bool> { { "favorite", true } });
            }

            return Json(new Dictionary<string, bool> { { "favorite", false } });

        }
        [HttpGet("{id}/comment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult Comment([FromRoute] int id, string comment)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            var film = _context.Films
                .Include(f=>f.Comments)
                .FirstOrDefault(f => f.LINK == id);
            var newComment = new Comment();
            newComment.Text = comment;
            newComment.ApplicationUser = user;
            film.Comments.Add(newComment);
            _context.SaveChanges();
            return Json(new Dictionary<string, bool> { { "success", true } });
        }
    }
}