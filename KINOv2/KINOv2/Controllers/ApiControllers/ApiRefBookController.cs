using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KINOv2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KINOv2.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/[action]")]
    [Authorize]
    public class ApiRefBookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApiRefBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NewsSerializer> GetNews()
        {
            var query = _context.News
                .ToList()
                .Select(x => new NewsSerializer
                {
                    PublishDate = x.PublishDate,
                    Name = x.Name,
                    Archived = x.Archived
                })
                .AsQueryable();
                    
            return query;
        }

        public IEnumerable<ExtendedUserSerializer> GetUsers()
        {
            var query = _context.Users
                .ToList()
                .Select(x => new ExtendedUserSerializer
                {
                    Name = x.Name,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed
                })
                .AsQueryable();

            return query;
        }

        public IEnumerable<DefaultRefBookSerializer> GetDirectors()
        {
            var query = _context.Directors
                .ToList()
                .Select(x => new DefaultRefBookSerializer
                {
                    Name = x.Name
                })
                .AsQueryable();

            return query;
        }

        public IEnumerable<DefaultRefBookSerializer> GetCountries()
        {
            var query = _context.Countries
                .ToList()
                .Select(x => new DefaultRefBookSerializer
                {
                    Name = x.Name
                })
                .AsQueryable();

            return query;
        }

        public IEnumerable<DefaultRefBookSerializer> GetQAs()
        {
            var query = _context.QAs
                .ToList()
                .Select(x => new DefaultRefBookSerializer
                {
                    Name = x.Name
                })
                .AsQueryable();

            return query;
        }

        public IEnumerable<DefaultRefBookSerializer> GetSubjects()
        {
            var query = _context.UserRequestSubjects
                .ToList()
                .Select(x => new DefaultRefBookSerializer
                {
                    Name = x.Name
                })
                .AsQueryable();

            return query;
        }

        public IEnumerable<AgeLimitSerializer> GetAgeLimits()
        {
            var query = _context.AgeLimits
                .ToList()
                .Select(x => new AgeLimitSerializer
                {
                    Value = x.Value
                })
                .AsQueryable();

            return query;
        }
    }
}