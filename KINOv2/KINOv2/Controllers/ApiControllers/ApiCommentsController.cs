using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KINOv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using KINOv2.Models.MainModels;
using KINOv2.Models;

namespace KINOv2.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/comments")]
    public class ApiCommentsController: Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/comments
        [HttpGet]
        public IEnumerable<CommentSerializer> GetComments(int? film = null)
        {
            if (film == null)
                return null;
            return _context.Comments
               .Include(c => c.ApplicationUser)
               .Include(c => c.Rating)
               .Where(c => c.FilmLINK == film)
               .ToList()
               .Select(c => new CommentSerializer
               {
                   LINK = c.LINK,
                   Text = c.Text,
                   Date = c.Date,
                   Rating = (int)c.Rating.Sum(r => r.Value),
                   BaseCommentLINK = c.BaseCommentLINK,
                   FilmLINK = c.FilmLINK,
                   User = new UserSerializer
                   {
                       Id = c.ApplicationUser.Id,
                       UserName = c.ApplicationUser.UserName,
                       ProfileImage = c.ApplicationUser.ProfileImage
                   }
               })
               .AsQueryable();
        }
        // GET: api/comments
        [HttpGet("withauth")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<CommentSerializer> GetCommentsWithAuth(int? film = null)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (film == null)
                return null;
            return _context.Comments
               .Include(c => c.ApplicationUser)
               .Include(c => c.Rating)
               .Where(c => c.FilmLINK == film)
               .ToList()
               .Select(c => new CommentSerializer
               {
                   LINK = c.LINK,
                   Text = c.Text,
                   Date = c.Date,
                   Rating = (int)c.Rating.Sum(r => r.Value),
                   BaseCommentLINK = c.BaseCommentLINK,
                   FilmLINK = c.FilmLINK,
                   YourRate = (int?)c.Rating.FirstOrDefault(r => r.ApplicationUserId == user?.Id)?.Value,
                   User = new UserSerializer
                   {
                       Id = c.ApplicationUser.Id,
                       UserName = c.ApplicationUser.UserName,
                       ProfileImage = c.ApplicationUser.ProfileImage
                   }
               })
               .AsQueryable();
        }
        [HttpGet("{id}/plus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult Plus([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            return Rate(user, id, 1);
        }
        [HttpGet("{id}/minus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult Minus([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return Json(new Dictionary<string, string> {
                    { "success", "false" },
                    { "error", "Неверные авторизационные данные"}
                });
            return Rate(user, id, -1);
        }
        private JsonResult Rate(ApplicationUser user, int? commentID, int value)
        {
            var comment = _context.Comments.Include(c=>c.Rating).FirstOrDefault(c => c.LINK == commentID);
            var rating = comment.Rating.FirstOrDefault(r => r.ApplicationUser == user);

            if (rating == null)
            {
                rating = new Rating();
                rating.Value = value;
                rating.ApplicationUser = user;
                comment.Rating.Add(rating);
                _context.SaveChanges();
                return Json(new Dictionary<string, string> {
                    { "success", "true" },
                });
            }
            else if ((int)rating.Value == value)
            {
                return Json(new Dictionary<string, string> {
                    { "success", "true" },
                    { "message", "Уже проголосовано" },
                });
            }
            else
            {
                rating.Value += value;
                _context.SaveChanges();
                return Json(new Dictionary<string, string> {
                    { "success", "true" },
                });
            }
        }
    }
}
