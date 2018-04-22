using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KINOv2.Data;
using KINOv2.Models.MainModels;
using KINOv2.Models;
using KINOv2.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using KINOv2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace KINOv2.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/profile")]
    public class ApiProfileController: Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("token")]
        //[HttpPost("/token")]
        public async Task Token(string username, string password)
        {
            //var username = Request.Form["username"];
            //var passwordHash = Request.Form["passwordHash"];
            password = password.Replace(' ', '+');
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: ApiAuthOptions.ISSUER,
                    audience: ApiAuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(ApiAuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(ApiAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(x => x.UserName == username);
            bool validated = PasswordHasher.VerifyIdentityV3Hash(password, user.PasswordHash);
            if (user != null && validated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ProfileSerializer GetProfileInfo()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return null;
            }
            var profileInfo = new ProfileSerializer
            {
                Username = user.UserName,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                Age = user.Age,
                City = user.City,
                Name = user.Name,
                SurName = user.SurName,
                About = user.About,
                SelectedFilmsVisible = user.SelectedFilmsVisible,
                PersonalInfoVisible = user.PersonalInfoVisible,
            };
            return profileInfo;
        }

        [HttpGet("history")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<OrderSerializer> GetHistory()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
            {
                return null;
            }
            var query = _context.Orders
                   .Include(o => o.Seats)
                   .ThenInclude(s => s.Session)
                   .ThenInclude(s => s.Film)
                   .Where(o => o.ApplicationUserId == user.Id)
                   .ToList()
                   .Select(o => new OrderSerializer
                   {
                       LINK = o.LINK,
                       Cost = o.Cost,
                       ValidationKey = o.ValidationKey,
                       Date = o.Date,
                       Username = user.UserName,
                       Seats = o.Seats.Select(seat => new SeatSerializer
                       {
                           LINK = seat.LINK,
                           Row = seat.Row,
                           Number = seat.Number,
                           IsBooked = seat.IsBooked,
                       }),
                       FilmName = o.Seats.First().Session.Film.Name,
                       FilmLINK = o.Seats.First().Session.Film.LINK,
                   })
                   .AsQueryable();
            //query.For(o => {
            //    var seat = _context.Seats.Include(s => s.Session).Where(s => s.OrderLINK == o.LINK).FirstOrDefault();
            //    seat.Session.Film = _context.Films.FirstOrDefault(f => seat.Session.FilmLINK == f.LINK);
            //    o.FilmLINK = seat.Session.Film.LINK;
            //    o.FilmName = seat.Session.Film.Name;
            //});
            return query;
        }
        
    }
}

