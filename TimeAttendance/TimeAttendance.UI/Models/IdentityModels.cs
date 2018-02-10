using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAttendance.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System;
using TimeAttendance.Domain;

namespace TimeAttendance.UI.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, Role, int, UserLogin, UserRole, UserClaim>//: DbContext
    {
        public ApplicationDbContext(): base("MyDbContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.Log = s => System.Diagnostics.Debug.Write(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("ta");
            modelBuilder.Entity<AppUser>().ToTable("user", "ta");
            modelBuilder.Entity<Role>().ToTable("role", "ta");
            modelBuilder.Entity<UserRole>().ToTable("user_role", "ta");
            modelBuilder.Entity<UserLogin>().ToTable("user_login", "ta");
            modelBuilder.Entity<UserClaim>().ToTable("user_claim", "ta");

            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
                 
        }

        public DbSet<Marks> Marks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}