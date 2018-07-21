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

            builder.Entity<FilmUser>()
                .HasKey(k => new { k.ApplicationUserId, k.FilmLINK });

            builder.Entity<FilmUser>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.FilmUsers)
                .HasForeignKey(x => x.ApplicationUserId);

            builder.Entity<FilmUser>()
                .HasOne(x => x.Film)
                .WithMany(x => x.FilmUsers)
                .HasForeignKey(x => x.FilmLINK);
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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<QA> QAs { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }
        public DbSet<UserRequestSubject> UserRequestSubjects { get; set; }
    }
}
