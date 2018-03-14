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
    [Route("api/session")]
    public class ApiSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/session
        [HttpGet]
        public IEnumerable<Session> GetSessions(int? film = null)
        {
            if (film == null)
                return _context.Sessions;
            else
                return _context.Sessions.Where(s => s.FilmLINK == film);
        }

        // GET: api/session/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = await _context.Sessions.SingleOrDefaultAsync(m => m.LINK == id);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }
        
        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.LINK == id);
        }
    }
}