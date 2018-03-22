using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KINOv2.Models;
using KINOv2.Data;
using KINOv2.Models.MainModels;
using Microsoft.EntityFrameworkCore;
using KINOv2.Models.ReferenceBooks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using KINOv2.Models.AdditionalEFEntities;
using System.Net;

namespace KINOv2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            DB = context;
            UserManager = userManager;
        }

        private ApplicationDbContext DB { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }

        public IActionResult Index()
        {
            IEnumerable<Film> films = DB.Films
                .Include(x => x.Genre)
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.AgeLimit)
                .Where(x => x.Archived != true)
                .Take(1);

            IEnumerable<Hall> halls = DB.Halls;

            ViewData["Halls"] = halls;
            ViewData["Featured"] = films.ToList();
      
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Film(int? id)
        {
            if (id is null)
                return View("Error");

            Film film = await DB.Films
                .Include(x => x.Genre)
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.AgeLimit)
                .Include(x => x.Rating)
                .Include(x => x.FilmUsers)
                .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.LINK == id)
                .SingleAsync();

            List<Session> sessions = DB.Sessions
                .Include(x => x.Hall)
                .Where(x => x.FilmLINK == film.LINK && x.Archived != true && x.SessionTime.Date == new DateTime(2017, 09, 28).Date)
                .ToList();

            Dictionary<string, List<Session>> sessionsByHall = new Dictionary<string, List<Session>>();
            foreach(Hall hall in DB.Halls)
            {
                sessionsByHall.Add(hall.Name, sessions.Where(x => x.Hall.Name == hall.Name).OrderBy(x => x.SessionTime).ToList());
            }

            List<Comment> comments = await DB.Comments
                .Include(x => x.ApplicationUser)
                .Include(x => x.BaseComment)
                .Include(x => x.Rating)
                .Where(x => x.FilmLINK == film.LINK)
                .ToListAsync();
            
            ViewData["Film"] = film;
            //ViewData["SelectedDate"] = new DateTime(2017, 09, 28).Date;
            ViewData["FilmSessions"] = sessionsByHall;
            ViewData["Title"] = film.Name;
            ViewData["Favorite"] = film.FilmUsers.Where(x => x.ApplicationUserId == UserManager.GetUserId(User)).Count() > 0 ? true : false;
            ViewData["CommentsCount"] = comments.Count;
            ViewData["Comments"] = comments;
            ViewData["FilmRated"] = film.Rating.Where(x => x.ApplicationUserId == UserManager.GetUserId(User)).Count() > 0 ? true : false;

            return View();
            
        }

        public async Task<IActionResult> Affiche(int page = 1)
        {
            int pageSize = 3;

            IQueryable<Film> films = DB.Films
                .Include(x => x.Genre)
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.AgeLimit)
                .Where(x => x.Archived != true);

            var count = await films.CountAsync();
            var selectedFilms = await films.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            ViewData["Films"] = selectedFilms;
            ViewData["PageView"] = pageView;

             return View();
        }

        public IActionResult Halls()
        {
            IEnumerable<Hall> halls = DB.Halls;
            ViewData["Halls"] = halls;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Sessions(int id =-1)
        {
            try
            {
                Session session = await DB.Sessions
                    .Include(x => x.Film)
                    .Include(x => x.Hall)
                    .Where(x => x.LINK == id && x.Archived != true)
                    .SingleAsync();
                
                if (session == null)
                    throw new KeyNotFoundException();

                session.Seats = DB.Seats.Where(s => s.SessionLINK == session.LINK).ToList();

                ViewData["Session"] = session;
            }
            catch
            {
                return View("Error");
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            Order order = new Order();
            string validationKey = GetRandomKey();
            while (DB.Orders.Where(o => o.ValidationKey == validationKey).Count() > 0)
            {
                validationKey = GetRandomKey();
            }
            var form = Request.Form;
            form.TryGetValue("session-cost", out StringValues scost);
            int cost = Convert.ToInt32(scost);
            cost *= form.Count - 2;
            string applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            order.ValidationKey = validationKey;
            order.ApplicationUserId = applicationUserId;
            order.Cost = cost;

            DB.Orders.Add(order);
            DB.SaveChanges();

            form.TryGetValue("session-link", out StringValues slink);
            int sessionLink = Convert.ToInt32(slink);

            foreach(var pair in form)
            { 
                if (pair.Key != "session-cost" && pair.Key != "session-link" && pair.Key != "__RequestVerificationToken")
                {
                    Seat newSeat = new Seat();
                    int value = Convert.ToInt32(pair.Value);
                    int row = value / 1000;
                    int number = value % 1000;

                    newSeat.Row = row;
                    newSeat.Number = number;
                    newSeat.IsBooked = true;
                    newSeat.OrderLINK = order.LINK;
                    newSeat.SessionLINK = sessionLink;

                    DB.Seats.Add(newSeat);
                    DB.SaveChanges();
                }
            }

            ViewBag.ValidationKey = validationKey;
            return View();

            
            string GetRandomKey()
            {
                Random rnd = new Random();
                string key = "";
                string chars = "0123456789-QWERTYUIOPASDFGHJKLZXCVBNM";
                for (int i = 0; i < 6; i++)
                {
                    int j = rnd.Next(0, chars.Length - 1);
                    key += chars[j];
                }
                return key;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int id, string msg, int replyid)
        {
            var film = await DB.Films.FindAsync(id);

            if (film == null)
                return null;

            Comment comment = new Comment
            {
                FilmLINK = film.LINK,
                ApplicationUserId = UserManager.GetUserId(User),
                Text = msg,
                Date = DateTime.Now
            };

            if (replyid != -1)
                comment.BaseCommentLINK = replyid;

            await DB.Comments.AddAsync(comment);
            await DB.SaveChangesAsync();

            var comments = await DB.Comments
                .Include(x => x.ApplicationUser)
                .Include(x => x.BaseComment)
                .Include(x => x.Rating)
                .Where(x => x.FilmLINK == id)
                .OrderBy(x => x.LINK)
                .ToListAsync();

            ViewData["Comments"] = comments;
            ViewData["CommentsCount"] = comments.Count;

            return PartialView("Comments");
        }

        [HttpGet]
        public IActionResult ToggleFavorite(int id)
        {
            var film = DB.Films.Include(x => x.FilmUsers)
                .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.LINK == id)
                .Single();

            var user = DB.Users.Find(UserManager.GetUserId(User));
            if(film.FilmUsers.Count > 0)
            if (film.FilmUsers.First(x => x.ApplicationUserId == user.Id) == null)
            {
                film.FilmUsers.Add(new FilmUser { ApplicationUserId = user.Id, FilmLINK = Convert.ToInt32(id) });
                DB.Entry(film).State = EntityState.Modified;
            }
            else
            {
                film.FilmUsers.Remove(film.FilmUsers.Where(x => x.ApplicationUserId == user.Id && x.FilmLINK == film.LINK).Single());
                DB.Entry(film).State = EntityState.Modified;
            }
            else
            {
                film.FilmUsers.Add(new FilmUser { ApplicationUserId = user.Id, FilmLINK = Convert.ToInt32(id) });
                DB.Entry(film).State = EntityState.Modified;
            }

            DB.SaveChanges();

            return null;
        }

        [HttpPost]
        public JsonResult RateFilm(int id, string score)
        {
            string status = "";
            string msg = "";
            

            var film = DB.Films
                .Include(x => x.Rating)
                .Where(x => x.LINK == id)
                .FirstOrDefault();

            var rating = film.Rating
                .Where(x => x.ApplicationUserId == UserManager.GetUserId(User))
                .FirstOrDefault();

            if(rating != null)
            {
                status = "ERR";
                msg = "Вы уже голосовали";
            }
            else
            {
                Rating rate = new Rating();
                rate.Value = Convert.ToDouble(score.Replace('.', ','));
                rate.ApplicationUserId = UserManager.GetUserId(User);
                film.Rating.Add(rate);

                DB.SaveChanges();
                status = "ОК";
                msg = "Ваш голос учтен";
            }

            return Json((status, msg));
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> RateComment(int id, bool? value)
        {
            string status = "";
            int msg = 0;

            if (value is null)
            {
                status = "ERR";
            }

            var comment = await DB.Comments
                .Include(x => x.Rating)
                .Where(x => x.LINK == id)
                .FirstAsync();

            var rating = comment.Rating
                .Where(x => x.ApplicationUserId == UserManager.GetUserId(User));

            if (rating.Count() > 0)
            {
                status = "ERR";
            }

            Rating rate = new Rating
            {
                Value = (value == true) ? 1 : -1,
                ApplicationUserId = UserManager.GetUserId(User)
            };

            comment.Rating.Add(rate);
            await DB.SaveChangesAsync();

            status = "OK";
            DB.Comments
                .Include(x => x.Rating)
                .Where(x => x.LINK == id)
                .FirstOrDefault()
                .Rating
                .ToList()
                .ForEach(x => msg += Convert.ToInt32(x.Value));

            return Json((status, msg));
        }
        

        public IActionResult FilmSessions(int filmid, string date)
        {
            if (!DateTime.TryParse(date, out DateTime selectedDate))
                return null;

            List<Session> sessions = DB.Sessions
                .Include(x => x.Hall)
                .Where(x => x.FilmLINK == filmid && x.Archived != true && x.SessionTime.Date == selectedDate.Date)
                .ToList();

            Dictionary<string, List<Session>> sessionsByHall = new Dictionary<string, List<Session>>();
            foreach (Hall hall in DB.Halls)
            {
                sessionsByHall.Add(hall.Name, sessions.Where(x => x.Hall.Name == hall.Name).OrderBy(x => x.SessionTime).ToList());
            }

            if (sessions.Count == 0)
                sessionsByHall = null;

            ViewData["FilmSessions"] = sessionsByHall;
            return PartialView("FilmSessions");
        }
    }
}
