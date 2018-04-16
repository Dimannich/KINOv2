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
        public IEnumerable<SessionSerializer> GetSessions(int? film = null)
        {
            var query = _context.Sessions
                    .Include(s => s.Hall)
                    .Include(s => s.Film)
                    .Include(s => s.Seats)
                    .ToList()
                    .Select(s => new SessionSerializer
                    {
                        LINK = s.LINK,
                        Film = s.Film.Name,
                        FilmLINK = s.Film.LINK,
                        SessionTime = s.SessionTime,
                        Hall = s.Hall.Name,
                        Cost = s.Cost,
                        Seats = s.Seats.Select(seat => new SeatSerializer
                        {
                            LINK = seat.LINK,
                            Row = seat.Row,
                            Number = seat.Number,
                            IsBooked = seat.IsBooked,
                        }),
                        Archived = s.Archived,
                    })
               .AsQueryable();
            if (film == null)
                return query;
            else
                return query.Where(s => s.FilmLINK == film);
            //else
             //   return _context.Sessions.Where(s => s.FilmLINK == film);
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
    public class SessionSerializer
    {
        public int LINK { get; set; }
        public string Film { get; set; }
        public int FilmLINK { get; set; }
        public DateTime SessionTime { get; set; }
        public string Hall { get; set; }
        public IEnumerable<SeatSerializer> Seats { get; set; }
        public int Cost { get; set; }
        public bool? Archived { get; set; }
    }
    public class SeatSerializer
    {
        public int LINK { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public bool IsBooked { get; set; }
    }
}