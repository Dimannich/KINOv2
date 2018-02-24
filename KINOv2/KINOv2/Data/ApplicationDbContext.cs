using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KINOv2.Models;
using KINOv2.Models.AdditionalEFEntities;
using System.Data.SqlClient;
using StoredProcedureEFCore;
using KINOv2.Models.ReferenceBooks;
using KINOv2.Models.MainModels;

namespace KINOv2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
          
        }

        public ApplicationDbContext():base()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Hall> Halls { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<AgeLimit> AgeLimits { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Order> Orders { get; set; }

        public static List<UserOrdersHistoryModel> GetUserHistoryAsync(string userID)
        {
            //SqlParameter param = new SqlParameter("@UserID", userID);
            //var result = await new ApplicationDbContext(null).Database.SqlQuery<UserOrdersHistoryModel>
            //    ("GetHistoryByUser @UserID", param).ToListAsync();
            //var result = new ApplicationDbContext().Database.ExecuteSqlCommand("GetHistoryByUser @UserID", param)
            List<UserOrdersHistoryModel> result = null;//= await new ApplicationDbContext().Database.ExecuteSqlCommand("exec GetHistoryByUser @UserID", param);

            new ApplicationDbContext().LoadStoredProc("GetHistoryByUser").AddParam("UserID", userID).Exec(x => result = x.ToList<UserOrdersHistoryModel>());

            return result;
        }


    }
}
