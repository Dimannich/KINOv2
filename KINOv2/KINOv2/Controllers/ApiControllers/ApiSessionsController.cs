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
        public IEnumerable<HallSerializer> GetSessions(
            int? film = null, 
            DateTime? date = null,
            DateTime? date_from = null,
            DateTime? date_to = null
            )
        {
            var where_predicates = new List<Predicate<Session>>();
            if (film != null)
                where_predicates.Add((s) => s.FilmLINK == film.Value);
            if (date != null)
                where_predicates.Add((s) => s.SessionTime.Date == date.Value.Date);
            else if (date_from != null && date_to != null)
                where_predicates.Add((s) => s.SessionTime.Date >= date_from.Value.Date && s.SessionTime.Date <= date_to.Value.Date);
            Predicate<Session> where = (session) => {
                var result = true;
                where_predicates.ForEach(pred =>
                {
                    result = result && pred(session);
                });
                return result;
            };
            var query = _context.Halls
                .ToList()
                .Select(h => new HallSerializer
                {
                    LINK = h.LINK,
                    Hall = h.Name,
                    sessions = _context.Sessions
                        .Include(s => s.Hall)
                        .Include(s => s.Film)
                        .Include(s => s.Seats)
                        .Where(s => where(s))
                        .ToList()
                        .Select(s => new SessionSerializer
                        {
                            LINK = s.LINK,
                            Film = s.Film.Name,
                            FilmLINK = s.Film.LINK,
                            Poster = s.Film.Poster,
                            SessionTime = s.SessionTime,
                            Hall = s.Hall.Name,
                            Cost = s.Cost,
                            Duration = s.Film.Duration,
                            Seats = s.Seats.Select(seat => new SeatSerializer
                            {
                                LINK = seat.LINK,
                                Row = seat.Row,
                                Number = seat.Number,
                                IsBooked = seat.IsBooked,
                            }),
                            Archived = s.Archived,
                        })
                })
                .Where(h => h.sessions.Count() > 0)
                .AsQueryable();
            return query;
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