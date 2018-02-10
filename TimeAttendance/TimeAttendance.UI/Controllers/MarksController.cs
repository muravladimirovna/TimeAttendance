using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeAttendance.Domain.Models;
using TimeAttendance.UI.Models;

namespace TimeAttendance.UI.Controllers
{

    [Authorize]
    public class MarksController : Controller
    {
        protected int Y = DateTime.Now.Year;  // фильтр за последний месяц
        //protected int M = DateTime.Now.Month - 1;
        //protected int D = DateTime.Now.Day;
        
        // временный костылик, чтобы увидеть хоть что-то =)
        protected int M = 06;
        protected int D = 01;

        //GET: /Marks/
        public async Task<ActionResult> Index(int? id)
        {
            int? Id;
            var ID = User.Identity.GetUserId<int>();
            var roles = await UserManager.GetRolesAsync(ID);
            if (id == null)
            {
                if (roles.Contains("admin"))
                {   // admin без параметра
                    Id = null;
                    ViewBag.UserId = null;
                    ViewBag.UserName = "всех пользователей";
                }else
                {   // user без параметра
                    Id = ID;
                    ViewBag.UserId = null;
                    ViewBag.UserName = User.Identity.Name;
                }
            }else
            {
                if (roles.Contains("admin"))
                {   // admin c параметром
                    Id = id.Value;
                    ViewBag.UserId = id.Value;
                    ViewBag.UserName = User.Identity.Name;
                }else
                {
                    if (id.Value == ID)
                    {   // user по своему id
                        Id = ID;
                        ViewBag.UserId = ID;
                        ViewBag.UserName = User.Identity.Name;
                    }else
                    {   // user по чужому id
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            using (var db = new ApplicationDbContext())
            {
                IQueryable<Marks> Marks;
                if (Id != null)
                {   // фильтруем по Id пользователя
                    Marks = db.Marks.Where(x => x.UserId == Id.Value && x.Coming_Date >= new DateTime(Y, M, D)).AsNoTracking();
                }else
                {   // не фильтруем, выводим всех пользователей
                    Marks = db.Marks.Where(x => x.Coming_Date >= new DateTime(Y, M, D)).AsNoTracking();
                }
                var list = from m in Marks
                                join u in db.Users.AsNoTracking() on m.UserId equals u.Id
                                join s in db.Users.AsNoTracking() on m.AuthorId equals s.Id
                                orderby m.Coming_Date descending
                                select new ShowMarks { Id = m.Id, Coming_Date = m.Coming_Date, Out_Date = m.Out_Date, UserName = u.UserName, UserId = u.Id, Author = s.UserName };
                var List = list.ToList();
                if (ViewBag.UserId == null)
                {   // выводим список отметок
                    return View(List);
                }else
                {   // выводим в Create (откл скрипты)
                    return PartialView(List);
                }
            }
        }

        //
        // GET: Marks/CreateUserMark
        public async Task<ActionResult> Create(int? id)
        {
            int FindId;
            var Id = User.Identity.GetUserId<int>();
            var roles = await UserManager.GetRolesAsync(Id);
            if (id != null)
            {
                if (roles.Contains("admin"))
                {
                    FindId = id.Value;
                }else
                {
                    if (id.Value == Id)
                    {
                        FindId = Id;
                    }else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }else
            {
                FindId = Id;
            }
            CreateMarkModel Create;
            using (var db = new ApplicationDbContext())
            {
                var ServerDateTime = db.Database.SqlQuery<DateTime>("SELECT 'now'::timestamp(0)").First();
                var mymark = (from b in db.Marks.Where(x => x.UserId == Id).ToList()
                              orderby b.Coming_Date descending
                              select new Marks { Id = b.Id, Coming_Date = b.Coming_Date, Out_Date = b.Out_Date }).First();
                if (mymark.Out_Date == null && mymark.Coming_Date.Month == DateTime.Now.Month && mymark.Coming_Date.Day < DateTime.Now.Day )
                {
                    Create = new CreateMarkModel { Date = mymark.Coming_Date.AddHours(+9), warning = true, ServerDate = ServerDateTime };
                }else
                {
                    Create = new CreateMarkModel { Date = DateTime.Now, warning = false, ServerDate = ServerDateTime };
                }
            }
            AppUser user = await UserManager.FindByIdAsync(FindId);
            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;
            return View(Create);
        }

        //
        // POST: Marks/Create
        [HttpPost]
        public async Task<ActionResult> Create(int? id, CreateMarkModel model)
        {
            int AuthorId;
            var Id = User.Identity.GetUserId<int>();
            var roles = await UserManager.GetRolesAsync(Id);
            if (id != null)
            {
                if (roles.Contains("admin"))
                {
                    AuthorId = Id;
                    Id = id.Value;
                }else
                {
                    if (id.Value == Id)
                    {
                        AuthorId = id.Value;
                        Id = id.Value;
                    }else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }else
            {
                AuthorId = Id;
            }
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    //var ServerDateTime = db.Database.SqlQuery<DateTime>("SELECT localtimestamp;").First();
                    var ServerDateTime = db.Database.SqlQuery<DateTime>("SELECT 'now'::timestamp(0)").First();

                    Marks mymark, newmark;
                    if (db.Marks.Where(x => x.UserId == Id).ToList().Count() != 0)
                    {
                        mymark = (from b in db.Marks.Where(x => x.UserId == Id).ToList()
                                      orderby b.Coming_Date descending
                                      select new Marks { Id = b.Id, Coming_Date = b.Coming_Date, Out_Date = b.Out_Date }).First();
                    }else
                    {
                        mymark = new Marks { UserId = Id, Coming_Date = model.Date, Out_Date = null, AuthorId = AuthorId };
                        newmark = mymark;
                        db.Marks.Add(newmark);
                    }
                    var date = model.Date;
                    if (date.Date == ServerDateTime.Date && date.Hour == ServerDateTime.Hour && date.Minute == ServerDateTime.Minute)
                    {
                        if (mymark.Out_Date == null)
                        {
                            newmark = db.Marks.Where(x => x.Coming_Date == mymark.Coming_Date).FirstOrDefault();
                            newmark.Out_Date = model.Date;
                        }else
                        {
                            if (mymark.Coming_Date.Date != ServerDateTime.Date)
                            {
                                newmark = new Marks { UserId = Id, Coming_Date = model.Date, Out_Date = null, AuthorId = AuthorId };
                                db.Marks.Add(newmark);
                            }else
                            {
                                return RedirectToAction("Create", new { id = Id });
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Create", new { id=Id } );
                    }else
                    {
                        if(model.warning == true)
                        {
                            if (mymark.Out_Date == null && model.Date.Day == mymark.Coming_Date.Day && model.Date.Month == ServerDateTime.Date.Month)
                            {
                                newmark = db.Marks.Where(x => x.Coming_Date == mymark.Coming_Date).FirstOrDefault();
                                newmark.Out_Date = model.Date;
                            }else
                            {
                                return RedirectToAction("Create", new { id = Id });
                            }
                            db.SaveChanges();
                            return RedirectToAction("Create", new { id = Id });
                        }
                        return RedirectToAction("Create", new { id = Id } );
                    }
                }
            }else
            {
                return RedirectToAction("Create", new { id = Id } );
            }
        }

        // GET: Marks/Report
        [Authorize(Roles = "admin")]
        public ActionResult CreateReport()
        {
            List<AddReport> List;
            using (var db = new ApplicationDbContext())
            {
                var list = db.Database.SqlQuery<AddReport>("SELECT \"Id\" AS \"UserId\", \"UserName\", firstname AS \"FirstName\", lastname AS \"LastName\", middlename AS \"MiddleName\" FROM ta.user ORDER BY \"UserName\" ASC; ").AsQueryable();
                List = list.ToList();
            }
            return View(List);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Report(AddReport model)
        {
            string query;
            List<ResultToReport> List;
            if (model.Date <= DateTime.Now)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.SaveChanges();
                    // норма часов/мес
                    var task = Convert.ToInt32(db.Database.SqlQuery<double>("SELECT CAST (SUM (count) AS integer) FROM ta.time WHERE Extract(MONTH from \"date\"::date ) = " + model.Date.Month + " AND Extract(YEAR from \"date\"::date ) = " + model.Date.Year + "; ").FirstOrDefault());
                    // выходные дни
                    var days = db.Database.SqlQuery<DateTime>("SELECT \"date\" FROM  ta.\"time\" WHERE count = 0 AND Extract(MONTH from \"date\") = " + model.Date.Month + " AND Extract(YEAR from \"date\"::date ) = " + model.Date.Year + "; ").AsQueryable().ToList();
                    // кол-во рабочих часов по датам
                    var Dates = await (db.Database.SqlQuery<Days>("SELECT \"date\", count AS sum FROM  ta.\"time\" WHERE Extract(MONTH from \"date\") = " + model.Date.Month + " AND Extract(YEAR from \"date\"::date ) = " + model.Date.Year + " ORDER BY \"date\"; ")).ToListAsync();
                    var OutList = db.Marks.Where(x => x.Out_Date == null).ToList();
                    foreach (var e in OutList)
                    {
                        var time = e.Coming_Date.AddHours(+9);
                        e.Out_Date = time;
                    }
                    if (model.UserId != 0)
                    {   // ФИО пользователя + сумма всех часов за месяц
                        query = "SELECT ta.mark.\"UserId\", ta.user.\"UserName\", ta.user.firstname, ta.user.lastname, ta.user.middlename, CAST (SUM ((EXTRACT(EPOCH FROM AGE(Out_Date, Coming_Date))-3600)/3600) AS INTEGER) FROM ta.mark INNER JOIN ta.user ON ta.mark.\"UserId\" = ta.user.\"Id\" WHERE ta.mark.\"UserId\" = " + model.UserId + " AND Extract(MONTH from Coming_Date::date ) = " + model.Date.Month + " AND Extract(YEAR from Coming_Date::date ) = " + model.Date.Year + " GROUP BY \"UserId\", ta.user.\"UserName\", ta.user.firstname, ta.user.lastname, ta.user.middlename; ";
                    }else
                    {
                        query = "SELECT ta.mark.\"UserId\", ta.user.\"UserName\", ta.user.firstname, ta.user.lastname, ta.user.middlename, CAST (SUM ((EXTRACT(EPOCH FROM AGE(Out_Date, Coming_Date))-3600)/3600) AS INTEGER) FROM ta.mark INNER JOIN ta.user ON ta.mark.\"UserId\" = ta.user.\"Id\" WHERE Extract(MONTH from Coming_Date::date ) = " + model.Date.Month + " AND Extract(YEAR from Coming_Date::date ) = " + model.Date.Year + " GROUP BY \"UserId\", ta.user.\"UserName\", ta.user.firstname, ta.user.lastname, ta.user.middlename ORDER BY ta.user.lastname;";
                    }
                    List = await (db.Database.SqlQuery<ResultToReport>(query)).ToListAsync();
                    if (List.Count != 0)
                    {
                        foreach (var b in List)
                        {
                            b.no = 0; b.dayoff = 0;
                            if (b.sum >= task)
                            {   // кол-во часов по датам пользователя
                                query = "SELECT \"date\", count AS sum FROM ta.\"time\" WHERE Extract(MONTH from ta.\"time\".\"date\"::date ) = " + model.Date.Month + " AND Extract(YEAR from \"date\"::date ) = " + model.Date.Year + " ORDER BY \"date\"; ";
                                b.Days = await (db.Database.SqlQuery<Days>(query)).ToListAsync();
                                // отработанные дни
                                b.dayall = b.Days.Where(m => m.sum != 0).Count();
                            }else
                            {   
                                query = "SELECT ta.\"time\".\"date\", mark.sum FROM ta.\"time\" LEFT JOIN (SELECT Coming_Date, CAST(EXTRACT(EPOCH FROM AGE(ta.mark.Out_Date, ta.mark.Coming_Date)) / 3600 AS integer) AS sum FROM ta.mark WHERE \"UserId\" = " + b.UserId + ") AS mark ON ta.\"time\".\"date\" = mark.Coming_Date::date WHERE Extract(MONTH from ta.\"time\".\"date\") = " + model.Date.Month + " AND Extract(YEAR from \"date\"::date ) = " + model.Date.Year + " ORDER BY ta.\"time\".\"date\"; ";
                                b.Days = await (db.Database.SqlQuery<Days>(query)).ToListAsync();// кол-во часов по датам
                                var qer = "SELECT coming_date::date FROM ta.mark WHERE Extract(MONTH from Coming_Date::date ) = " + model.Date.Month + " AND \"UserId\" = " + b.UserId + " AND CAST(((EXTRACT(EPOCH FROM AGE(Out_Date, Coming_Date)) - 3600) / 3600) AS INTEGER) < 8 AND Coming_Date::time > '08:15:00'; ";
                                b.latelist = db.Database.SqlQuery<DateTime>(qer).ToList(); // опоздания
                                b.lateness = b.latelist.Count();
                                //qer = "SELECT out_date::date FROM ta.mark WHERE Extract(MONTH from Coming_Date::date ) = " + model.Date.Month + " AND \"UserId\" = " + b.UserId + " AND CAST(((EXTRACT(EPOCH FROM AGE(Out_Date, Coming_Date)) - 3600) / 3600) AS INTEGER) < 8 AND out_date::time < '17:00:00'; ";
                                //b.earlylist = db.Database.SqlQuery<DateTime>(qer).ToList();
                                //b.early = b.earlylist.Count();
                                // всего отработал дней
                                b.dayall = b.Days.Where(m => m.sum != null).Count();
                                foreach (var m in b.Days)
                                {
                                    if (m.sum == null && !days.Contains(m.date))
                                    {   // прогулы
                                        b.no++; 
                                    }else
                                    {
                                        if (days.Contains(m.date) && m.sum >=8)
                                        {   // кол-во отработанных выходных
                                            b.dayoff++; 
                                        }
                                    }
                                }
                            }
                        }
                    }else
                    {   // если ни один пользователь не отмечен
                        query = "SELECT \"Id\", \"UserName\",firstname, lastname, middlename FROM ta.user ORDER BY \"UserName\"; ";
                        List = await (db.Database.SqlQuery<ResultToReport>(query)).ToListAsync(); 
                    }
                    // название месяца
                    var mname = db.Database.SqlQuery<string>("select to_char(\"date\", 'TMMonth') FROM ta.\"time\" WHERE Extract(MONTH from \"date\"::date) = " + model.Date.Month + "; ").FirstOrDefault();
                    ViewBag.Info = new ReportInfo { Task = task, Days = days, All = Dates, Count = Dates.Where(m => m.sum != 0).Count(), MName = mname };
                    foreach (var e in OutList)
                    {
                        e.Out_Date = null;
                    }
                    db.SaveChanges();
                    return View(List);
                }
            }else
            {   // некорректная дата
                using (var db = new ApplicationDbContext())
                {
                    List<AddReport> NList;
                    var list = db.Database.SqlQuery<AddReport>("SELECT \"Id\" AS \"UserId\", \"UserName\", firstname AS \"FirstName\", lastname AS \"LastName\", middlename AS \"MiddleName\" FROM ta.user ORDER BY \"UserName\" ASC; ").AsQueryable();
                    NList = list.ToList();
                    return RedirectToAction("CreateReport", NList);
                }
            }
        }
        

        // GET: Marks/Edit
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var mark = (from m in db.Marks.Where(x => x.Id ==id)
                           join s in db.Users.AsQueryable<AppUser>() on m.UserId equals s.Id
                           select new ShowMarks { Id = m.Id, Coming_Date = m.Coming_Date, Out_Date = m.Out_Date, UserName = s.UserName, UserId = s.Id }).FirstOrDefault();
                var list = new EditMark { Id = mark.Id, Coming_Date = mark.Coming_Date, Out_Date = mark.Out_Date, UserName = mark.UserName, UserId = mark.UserId };
                return PartialView(list);
            }
        }

        // POST: Marks/Edit
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(EditMark model)
        {
                DateTime result;
                if (model.Out_Date != null)
                {
                if (DateTime.TryParse(model.Coming_Date.ToString(), out result) && DateTime.TryParse(model.Out_Date.ToString(), out result) && model.Coming_Date < model.Out_Date)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var editmark = db.Marks.Where(x => x.Id == model.Id).FirstOrDefault();
                        editmark.Coming_Date = model.Coming_Date;
                        editmark.Out_Date = model.Out_Date;
                        editmark.AuthorId = User.Identity.GetUserId<int>();
                        db.SaveChanges();
                    }
                    return RedirectToAction("Create", new { id = model.UserId });
                }
                else
                {
                    return RedirectToAction("Create", new { id = model.UserId });
                }
            }else
            {
                if (DateTime.TryParse(model.Coming_Date.ToString(), out result) )
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var editmark = db.Marks.Where(x => x.Id == model.Id).FirstOrDefault();
                        editmark.Coming_Date =model.Coming_Date;
                        editmark.Out_Date = null;
                        editmark.AuthorId = User.Identity.GetUserId<int>();
                        db.SaveChanges();
                    }
                    return RedirectToAction("Create", new { id = model.UserId });
                }else
                {
                    return RedirectToAction("Create", new { id = model.UserId });
                }
            }
        }

        private ApplicationUserManager _userManager;
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
    }
}
