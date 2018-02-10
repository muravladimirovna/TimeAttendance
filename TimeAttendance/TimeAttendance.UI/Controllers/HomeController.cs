using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Domain.Models;
using TimeAttendance.Domain;

namespace TimeAttendance.UI.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext db;
        public HomeController()
        {
            db = new MyDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}