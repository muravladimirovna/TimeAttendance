using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Domain.Models;
using TimeAttendance.UI.Models;

namespace TimeAttendance.UI.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db;

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var users = UserManager.Users.AsNoTracking().ToList();
            ViewBag.Users = users;
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

        //
        //GET: /Users/Edit              
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                using (db = new ApplicationDbContext())
                {
                    var view = new EditViewModel { id = user.Id, username = user.UserName, firstname = user.FirstName, middlename = user.MiddleName, lastname = user.LastName, email = user.Email, phonenumber = user.PhoneNumber };
                    var roles = await UserManager.GetRolesAsync(id);
                    var addroles = (from b in db.Roles.ToList()
                                   select b.Name).ToList();
                    ViewBag.Roles = roles;
                    var AddRoles = addroles.Except(roles).ToList();
                    ViewBag.AddRoles = AddRoles;
                    return View(view);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //
        //POST: /Users/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(int id, EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ViewBag.Roles = await UserManager.GetRolesAsync(id);
                AppUser user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Email = model.email;
                    user.FirstName = model.firstname;
                    user.LastName = model.lastname;
                    user.MiddleName = model.middlename;
                    user.PhoneNumber = model.phonenumber;
                    IdentityResult validEmail
                        = await UserManager.UserValidator.ValidateAsync(user);

                    if (!validEmail.Succeeded)
                    {
                        AddErrorsFromResult(validEmail);
                    }
                    IdentityResult validPass = null;
                    if ((validEmail.Succeeded && validPass == null) /*|| (validEmail.Succeeded && password != string.Empty && validPass.Succeeded || phonenumber != null)*/)
                    {
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        //добавление роли
                        if (model.addrole != null)
                        {
                            await UserManager.AddToRoleAsync(id, model.addrole);
                            return RedirectToAction("Index");
                        }//удаление роли
                        if (model.delrole != null)
                        {
                            UserManager.RemoveFromRole(id, model.delrole);
                            return RedirectToAction("Index");
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }else
                        {
                            AddErrorsFromResult(result);
                            return View(model);
                        }
                    }
                }else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
                return View(user);
            }else
            {
                return View(model);
                }
            }

        //GET
        //[Authorize(Roles = "admin")]
        //public async Task<ActionResult> EditRoles(int id)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        var roles = await UserManager.GetRolesAsync(id);
        //        var addroles = (from m in db.Roles.ToList()
        //                       join s in roles on m.Name equals s into ps
        //                       from p in ps.DefaultIfEmpty()
        //                       select new Role { Name = p }).ToList();
        //        ViewBag.Roles = roles;
        //        ViewBag.AddRoles = addroles;
        //        return View();
        //    }
        //}

        ////POST
        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //public async Task<ActionResult> EditRoles(int id, EditRoles model)
        //{
        //    return View(model);
        //}

//
//GET: /Users/Create
public ActionResult Create()
        {
            return View();
        }

        //
        //POST: /Users/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model, string role)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { FirstName = model.FirstName, LastName = model.FirstName, MiddleName = model.MiddleName, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (role != null)
                    {
                        await UserManager.AddToRoleAsync(user.Id, role);
                    }
                    return RedirectToAction("Index", "Users");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult ChangePassword(int id)
        {
            return View();
        }

        //
        // POST: /Users/ChangePassword
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(int id, ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrorsFromResult(result);
            return View(model);
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        
        [Authorize(Roles = "admin")]
        //// GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //// POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}
