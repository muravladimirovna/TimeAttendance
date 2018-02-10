using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TimeAttendance.Domain.Models;

namespace TimeAttendance.Domain
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(): base()
        {
            Configuration.LazyLoadingEnabled = false;
            Database.Log = s => System.Diagnostics.Debug.Write(s);
        }
        //public DbSet<Marks> Marks { get; set; }
    }
}