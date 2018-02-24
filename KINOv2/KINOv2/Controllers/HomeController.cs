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

namespace KINOv2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            DB = context;
            UserManager = userManager;
        }

        private ApplicationDbContext DB{ get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }

        public IActionResult Index()
        {
            IEnumerable<Film> films = DB.Films
                .Include(x => x.Genre)
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.AgeLimit)
                .Where(x => x.Archived != true);

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
            ViewData["Message"] = "Your contact page.";

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
                .Where(x => x.LINK == id).SingleAsync();

            IEnumerable<Session> sessions = DB.Sessions
                .Include(x => x.Hall)
                .Where(x => x.FilmLINK == film.LINK && x.Archived != true);

            ViewData["Film"] = film;
            ViewData["Sessions"] = sessions;
            ViewData["Title"] = film.Name;

            return View();
            
        }

        public IActionResult Affiche()
        {
            IEnumerable<Film> films = DB.Films
                .Include(x => x.Genre)
                .Include(x => x.Director)
                .Include(x => x.Country)
                .Include(x => x.AgeLimit)
                .Where(x => x.Archived != true);

            ViewData["Films"] = films;
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
    }
}
