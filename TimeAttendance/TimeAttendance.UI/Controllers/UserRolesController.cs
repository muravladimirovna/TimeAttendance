using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class UserRolesController : Controller
    {
        //private ApplicationDbContext db;// = new MyDbContext();
        private ApplicationUserManager _userManager;

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            //var userroles = db.UserRoles.Include("User").Include("Role").ToList();
            var userroles = UserManager.Users.Include(x => x.Roles).AsNoTracking().ToList();
            ViewBag.UserRoles = userroles;
            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //// GET: UserRoles/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: UserRoles/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserId,RoleId")] UserRole userRole)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UserRoles.Add(userRole);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(userRole);
        //}

        //    // GET: UserRoles/Edit/5
        //    public ActionResult Edit(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        UserRole userRole = db.UserRoles.Find(id);
        //        if (userRole == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(userRole);
        //    }

        //    // POST: UserRoles/Edit/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Edit([Bind(Include = "Id,UserId,RoleId")] UserRole userRole)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(userRole).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        return View(userRole);
        //    }

        //    // GET: UserRoles/Delete/5
        //    public ActionResult Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        UserRole userRole = db.UserRoles.Find(id);
        //        if (userRole == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(userRole);
        //    }

        //    // POST: UserRoles/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteConfirmed(int id)
        //    {
        //        UserRole userRole = db.UserRoles.Find(id);
        //        db.UserRoles.Remove(userRole);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }
        }
    }
