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
using KINOv2.Models.ManageViewModels;
using Microsoft.Extensions.DependencyInjection;
using KINOv2.Controllers.Kassa;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace KINOv2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IServiceProvider serviceProvider)
        {
            DB = context;
            UserManager = userManager;
            RoleManager = roleManager;
            ServiceProvider = serviceProvider;
        }

        private ApplicationDbContext DB { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private IServiceProvider ServiceProvider { get; set; }

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
                .Include(x => x.Comments)
                .ThenInclude(c => c.ApplicationUser)
                .Include(x => x.Comments)
                .ThenInclude(c => c.Rating)
                .Include(x => x.Sessions)
                .ThenInclude(s => s.Hall)
                .Include(x => x.FilmUsers)
                .ThenInclude(x => x.ApplicationUser)
                .Where(x => x.LINK == id)
                .SingleAsync();


            
            Dictionary<string, List<Session>> sessionsByHall = new Dictionary<string, List<Session>>();
            foreach(Hall hall in DB.Halls)
            {
                sessionsByHall.Add(hall.Name, film.Sessions
                    .Where(x => x.Hall.Name == hall.Name 
                    && x.Archived == false 
                    && x.SessionTime.Date == DateTime.Now)
                    .OrderBy(x => x.SessionTime)
                    .ToList()
                    );
            }
            
            ViewData["Film"] = film;
            ViewData["FilmSessions"] = sessionsByHall;
            ViewData["Title"] = film.Name;
            ViewData["Favorite"] = film.FilmUsers.Where(x => x.ApplicationUserId == UserManager.GetUserId(User)).Count() > 0 ? true : false;
            ViewData["CommentsCount"] = film.Comments.Count;
            ViewData["Comments"] = film.Comments;
            ViewData["FilmRated"] = film.Rating.Where(x => x.ApplicationUserId == UserManager.GetUserId(User)).Count() > 0 ? true : false;

            return View();
            
        }

        public async Task<IActionResult> Affiche(int page = 1)
        {
            int pageSize = 4;

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
        [Authorize]
        public IActionResult CreateOrder([FromForm] IFormCollection form)
        {
            string applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = DB.Users.FirstOrDefault(u => u.Id == applicationUserId);
            if (user == null)
            {
                return Error();
            }
            return CreateOrderBase(form, user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateOrderMobile([FromForm] IFormCollection form)
        {
            var user = DB.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return Error();
            }
            return CreateOrderBase(form, user, 1);
        }

        private IActionResult CreateOrderBase(IFormCollection form, ApplicationUser user, int mobile = 0)
        {
            Order order = new Order();
            string validationKey = GetRandomKey();
            while (DB.Orders.Where(o => o.ValidationKey == validationKey).Count() > 0)
            {
                validationKey = GetRandomKey();
            }


            form.TryGetValue("session-link", out StringValues slink);
            int sessionLink = Convert.ToInt32(slink);
            var session = DB.Sessions.Include(s => s.Film).Include(s => s.Hall).FirstOrDefault(s => s.LINK == sessionLink);
            if (session == null)
                return Error();

            var totalCost = session.Cost * (form.Count - 3);

            order.ValidationKey = validationKey;
            order.ApplicationUser = user;
            order.Cost = totalCost;
            order.Date = DateTime.Now;
            order.Paid = false;
            order.PayStatus = "Ожидается оплата";

            DB.Orders.Add(order);
            DB.SaveChanges();

            List<Seat> seats = new List<Seat>();
            string seatsString = "";
            foreach (var pair in form)
            {
                if (pair.Key != "session-cost" && pair.Key != "session-link" && pair.Key != "__RequestVerificationToken")
                {
                    Seat newSeat = new Seat();
                    int value = Convert.ToInt32(pair.Value);
                    int row = value / 1000;
                    int number = value % 1000;

                    // проверяем, не забронькал ли кто место пока мы прохлаждались
                    var seat = DB.Seats.FirstOrDefault(s => s.Row == row && s.Number == number && s.SessionLINK == sessionLink);
                    if (seat != null)
                        return Error();

                    newSeat.Row = row;
                    newSeat.Number = number;
                    newSeat.IsBooked = true;
                    newSeat.OrderLINK = order.LINK;
                    newSeat.SessionLINK = sessionLink;

                    seats.Add(newSeat);
                    seatsString += $"{row} ряд {number} место, ";
                }
            }
            DB.Seats.AddRange(seats);
            DB.SaveChanges();
            seatsString = seatsString.TrimEnd(' ').TrimEnd(',');


            ViewBag.filmName = session.Film.Name;
            ViewBag.seatsString = seatsString;
            ViewBag.total = totalCost + ".00";
            ViewBag.date = session.SessionTime;
            ViewBag.hallName = session.Hall.Name;
            ViewBag.orderLink = order.LINK;
            ViewBag.mobile = mobile;
            string hashBae = $"{KassaOptions.MerchantLogin}:{ViewBag.total}:{order.LINK}:{KassaOptions.Pass1}";
            // build CRC value
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(hashBae));

            StringBuilder sbSignature = new StringBuilder();
            foreach (byte b in bSignature)
                sbSignature.AppendFormat("{0:x2}", b);

            string sCrc = sbSignature.ToString();
            ViewBag.hash = sCrc;
            ViewBag.mrhLogin = KassaOptions.MerchantLogin;
            ViewBag.mobile = 0;
            var url = "https://auth.robokassa.ru/Merchant/Index.aspx/?" +
                "MerchantLogin=" + KassaOptions.MerchantLogin +
                "&OutSum=" + totalCost + ".00" +
                "&InvoiceID=" + order.LINK +
                "&Description=" + HttpUtility.UrlEncode(seatsString) +
                "&SignatureValue=" + sCrc +
                "&IsTest=1";
            return Redirect(url);
            

        }

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
        public async Task<IActionResult> Profile(string username)
        {
            var user = await UserManager.Users
                .Where(x => x.UserName == username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                //throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
                return Error();
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                Age = user.Age,
                City = user.City,
                Name = user.Name,
                SurName = user.SurName,
                ProfileImage = user.ProfileImage,
                About = user.About,
                PersonalInfoVisible = user.PersonalInfoVisible,
                SelectedFilmsVisible = user.SelectedFilmsVisible,
                StatusMessage = ""
            };

            var _user = DB.Users.Where(x => x.Id == user.Id).Include(x => x.FilmUsers).ThenInclude(x => x.Film).First();

            List<Film> films = new List<Film>();
            foreach (var item in _user.FilmUsers)
            {
                films.Add(item.Film);
            }
            model.Films = films;
            
            if(User.Identity.IsAuthenticated && user.Id == (await UserManager.GetUserAsync(User)).Id)
            {
                return RedirectToActionPreserveMethod("Index", "Manage");
            }

            return View(model);
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
            if (film.FilmUsers.FirstOrDefault(x => x.ApplicationUserId == user.Id) == null)
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
                status = "OK";
                msg = "Ваш голос учтен";
            }

            var localRating = 0;
            foreach(var filmRate in film.Rating)
            {
                localRating += (int)filmRate.Value;
            }

            film.LocalRating = localRating / film.Rating.Count;
            DB.Entry(film).State = EntityState.Modified;

            DB.SaveChanges();

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
                return Json((status, msg));
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
                return Json((status, msg));
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
