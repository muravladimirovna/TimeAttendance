using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Domain;
using TimeAttendance.Domain.Models;
using TimeAttendance.UI.Models;

namespace TimeAttendance.UI.Controllers
{
    public class RolesController : Controller
    {
        //private ApplicationDbContext db;

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var roles = db.Roles.ToList();//= new List<Role>(); //
            ViewBag.Roles = roles;
            return View();
        }

        // GET: Roles/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Roles/Create
        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name")] Role role)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Roles.Add(role);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(role);
        //}

        //// GET: Roles/Edit/5
        //[Authorize(Roles = "admin")]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Role role = null;// db.Roles.Find(id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name")] Role role)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(role).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(role);
        //}

        // GET: Roles/Delete/5
        //[Authorize(Roles = "admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Role role = null;// db.Roles.Find(id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        //// POST: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Role role = db.Roles.Find(id);
        //    db.Roles.Remove(role);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
