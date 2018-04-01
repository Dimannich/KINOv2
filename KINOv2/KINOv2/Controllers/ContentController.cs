using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KINOv2.Data;
using KINOv2.Models.ContentManageViewModels;
using KINOv2.Models.MainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KINOv2.Controllers
{
    public class ContentController : Controller
    {
        public ContentController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            DB = context;
            AppEnvironment = hostingEnvironment;
        }

        private ApplicationDbContext DB { get; set; }
        private IHostingEnvironment AppEnvironment { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        //
        //GET: /Content/FilmManage
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FilmManage(int? id)
        {
            Film film = null;

            if (!(id is null))
            {
                film = await DB.Films.FindAsync(id);

                if (film is null)
                    return View("Error");
            }
            
            SelectList genresList = new SelectList(DB.Genres.ToList(), "LINK", "Name");
            ViewBag.Genres = genresList;
            SelectList directorsList = new SelectList(DB.Directors.ToList(), "LINK", "Name");
            ViewBag.Directors = directorsList;
            SelectList countriesList = new SelectList(DB.Countries.ToList(), "LINK", "Name");
            ViewBag.Countries = countriesList;
            SelectList ageLimitsList = new SelectList(DB.AgeLimits.ToList(), "LINK", "Value");
            ViewBag.AgeLimits = ageLimitsList;

            FilmManageViewModel model = new FilmManageViewModel();
            model.Film = film;
            return View(model);
        }

        //
        //POST: /Content/FilmManage
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FilmManage(FilmManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.Film.Poster.EndsWith(".jpg"))
                    model.Film.Poster = model.Film.Poster + ".jpg";

                var film = await DB.Films.FindAsync(model.Film.LINK);
                if (film != null)
                {
                    foreach (var prop in film.GetType().GetProperties())
                    {
                        prop.SetValue(film, model.Film.GetType().GetProperty(prop.Name).GetValue(model.Film));
                    }
                    DB.Entry(film).State = EntityState.Modified;
                }
                else
                {

                    DB.Entry(model.Film).State = EntityState.Added;
                }
            }
            else
            {
                SelectList genresList = new SelectList(DB.Genres.ToList(), "LINK", "Name");
                ViewBag.Genres = genresList;
                SelectList directorsList = new SelectList(DB.Directors.ToList(), "LINK", "Name");
                ViewBag.Directors = directorsList;
                SelectList countriesList = new SelectList(DB.Countries.ToList(), "LINK", "Name");
                ViewBag.Countries = countriesList;
                SelectList ageLimitsList = new SelectList(DB.AgeLimits.ToList(), "LINK", "Value");
                ViewBag.AgeLimits = ageLimitsList;
                return View(model);
            }

            if (model.UploadedFile != null
                && (model.UploadedFile.FileName.EndsWith(".jpg")))
            //model.UploadedFile.SaveAs(Server.MapPath("~/Content/Images/Posters/" + model.Film.Poster));
            {
                using (var fileStream = new FileStream((AppEnvironment.WebRootPath + "/images/Posters/" + model.Film.Poster), FileMode.Create))
                {
                   await model.UploadedFile.CopyToAsync(fileStream);
                }
            }

            await DB.SaveChangesAsync();
            return RedirectToAction("Affiche", "Home");
        }

        //
        //GET: /Content/SessionManage
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SessionManage(int? id, int? film)
        {
            Session session = null;

            if(!(id is null))
            {
                session = await DB.Sessions
                    .Where(x => x.LINK == id)
                    .Include(x => x.Film)
                    .FirstOrDefaultAsync();

                if (session.LINK == -1)
                    return View("Error");
            }

            if(!(film is null))
            {
                ViewBag.Film = DB.Films.Find(film);
            }
            //if (int.TryParse(Request.Query["id"].ToString(), out film))
            //{
            //    ViewBag.film = film;
            //    ViewBag.FilmName = DB.Films.Find(film).Name;
            //}
            //string link = Request.Query["id"].ToString();
            //if (link != null)
            //{
            //    if (!Int32.TryParse(link, out int l))
            //    {
            //        return View("Error");
            //    }
            //    session = await DB.Sessions.FindAsync(l);
            //    if (session == null)
            //        return View("Error");
            //    if (session.Archived == true)
            //        return View("Error");
            //    session.Film = await DB.Films.FindAsync(session.FilmLINK);
            //}

            SelectList filmsList = new SelectList(DB.Films.Where(x => x.Archived != true).ToList(), "LINK", "Name");
            ViewBag.Films = filmsList;
            SelectList hallsList = new SelectList(DB.Halls.ToList(), "LINK", "Name");
            ViewBag.Halls = hallsList;

            return View(session);
        }

        //
        //POST: /Content/SessionManage
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SessionManage(Session model)
        {
            //var context = ApplicationDbContext.Create();

            //string dateString = Request.Query["date_field"].ToString();
            //bool isDateValid = DateTime.TryParse(dateString, out DateTime date);
            if (ModelState.IsValid)// && isDateValid)
            {
                //model.SessionTime = date;
                var session = await DB.Sessions.FindAsync(model.LINK);
                if (session != null)
                {
                    foreach (var prop in session.GetType().GetProperties())
                    {
                        prop.SetValue(session, model.GetType().GetProperty(prop.Name).GetValue(model));
                    }

                    DB.Entry(session).State = EntityState.Modified;
                }
                else
                {
                    DB.Entry(model).State = EntityState.Added;
                }
            }
            //else
            //{
                //if (!isDateValid)
                //{
                //    model.SessionTime = DateTime.Now;
                //}
                //else model.SessionTime = date;
                //SelectList filmsList = new SelectList(DB.Films.Where(x => x.Archived != true).ToList(), "LINK", "Name");
                //ViewBag.Films = filmsList;
                //SelectList hallsList = new SelectList(DB.Halls.ToList(), "LINK", "Name");
                //ViewBag.Halls = hallsList;
                //if (model.FilmLINK != null)
                //    model.Film = await DB.Films.FindAsync(model.FilmLINK);
                //return View(model);
            //}
            await DB.SaveChangesAsync();

            return RedirectToAction("Affiche", "Home");
        }

        //
        //GET: /Content/ArchiveFilm
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ArchiveFilm(int? id)
        {
            //var context = ApplicationDbContext.Create();

            if (id == null)
            {
                return View("Error");
            }

            Film film = await DB.Films.FindAsync(id);

            if (film == null)
            {
                return View("Error");
            }

            var sessions = DB.Sessions.Where(x => x.FilmLINK == film.LINK);
            if (sessions == null)
                film.Archived = true;
            else
            {
                foreach (var s in sessions)
                {
                    if (s.SessionTime > DateTime.Now)
                    {
                        return View("Error");
                    }
                }
            }

            DB.Entry(film).State = EntityState.Modified;
            foreach (var session in DB.Sessions)
            {
                if (session.FilmLINK == film.LINK)
                {
                    session.Archived = true;
                    DB.Entry(session).State = EntityState.Modified;
                }
            }
            await DB.SaveChangesAsync();
            return RedirectToAction("Affiche", "Home");
        }

        //
        //GET: /Content/ArchiveSession
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ArchiveSession(int? id)
        {
            //var context = ApplicationDbContext.Create();

            if (id == null)
            {
                return View("Error");
            }

            Session session = await DB.Sessions.FindAsync(id);

            if (session == null)
            {
                return View("Error");
            }


            int filmID = (int)session.FilmLINK;

            if (session.SessionTime > DateTime.Now)
            {
                return View("Error");
            }

            session.Archived = true;
            DB.Entry(session).State = EntityState.Modified;
            await DB.SaveChangesAsync();
            return RedirectToAction("Film", "Home", new { id = filmID });
        }
    }
}