using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KINOv2.Data;

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
               .Where(c => c.FilmLINK == film)
               .ToList()
               .Select(c => new CommentSerializer
               {
                   LINK = c.LINK,
                   Text = c.Text,
                   Date = c.Date,
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
        
    }
}
