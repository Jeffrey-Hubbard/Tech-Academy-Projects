using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleUsers.Areas.Employer.ViewModels;
using ScheduleUsers.Models;

namespace ScheduleUsers.Areas.Employer.Controllers
{
    public class ScheduleController : Controller
    {
        //Generate User's first and last name in navigation bar
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    string fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                    ViewData.Add("FullName", fullName);
                }
            }
            base.OnActionExecuted(filterContext);
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employer/Schedule
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.User);
            return View(schedules.ToList());
        }

        // GET: Employer/Schedule/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Employer/Schedule/Create
        public ActionResult Create(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleViewModel scheduleVM = new ScheduleViewModel(Id);
            ViewBag.WorkTypeList = GetWorkTypeList();
            return View(scheduleVM);
        }

        public List<SelectListItem> GetWorkTypeList()
        {
            var getWorkList = db.WorkPeriods.Select(x => x.WorkType).Distinct().ToList();
            var WorkTypeList = new List<SelectListItem>();
            bool listContainsAddNewWorktype = false;
            foreach (string worktype in getWorkList)
            {
                if (worktype != null)
                {
                    if (worktype == "Add New Worktype") { listContainsAddNewWorktype = true; }
                    WorkTypeList.Add(new SelectListItem { Text = worktype, Value = worktype });
                }
            }
            if (listContainsAddNewWorktype == false)
            {
                WorkTypeList.Add(new SelectListItem { Text = "Add New Worktype", Value = "Not Selected" });
            }
            return WorkTypeList;
        }

        public ScheduleViewModel Change(ScheduleViewModel scheduleVM)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Where(x => x.Id == scheduleVM.UserId).First();
                scheduleVM.FirstName = user.FirstName;
                scheduleVM.LastName = user.LastName;
            }

            if (scheduleVM.WorkPeriods.Count > scheduleVM.ScheduleLength)
            {
                int adjustedLength = scheduleVM.WorkPeriods.Count - scheduleVM.ScheduleLength;
                for (int i = 0; i < adjustedLength; i++)
                {
                    //remove last workperiod
                    scheduleVM.WorkPeriods.RemoveAt(scheduleVM.WorkPeriods.Count - 1);
                }

            }
            else if (scheduleVM.WorkPeriods.Count < scheduleVM.ScheduleLength) {
                int adjustedLength = scheduleVM.ScheduleLength - scheduleVM.WorkPeriods.Count;
                for (int i = 0; i < adjustedLength; i++)
                {
                    // add one work period
                    scheduleVM.WorkPeriods.Add(new WorkPeriod());
                    // set the default start date of the newly added work period
                    int offsetIndexBy = scheduleVM.WorkPeriods.Count - 1;
                    scheduleVM.WorkPeriods[offsetIndexBy].StartTime = scheduleVM.ScheduleStartDay.Value.AddDays(offsetIndexBy);
                }

            }

            return scheduleVM;
        }

        // POST: Employer/Schedule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Notes,ScheduleStartDay,ScheduleEndDay,ScheduleLength,Repeating,UserId,WorkPeriods")] ScheduleViewModel scheduleVM, string submit)
        {
            if (ModelState.IsValid)
            {              
                switch (submit)
                {
                    case "Change Schedule":
                        ScheduleViewModel newScheduleVM = Change(scheduleVM);
                        //check to see if viewbag added worktype exists in worktypelist
                        ViewBag.WorkTypeList = GetWorkTypeList();
                        int listLength = GetWorkTypeList().Count;
                        // loop through worktypes saved in the viewmodel. 
                        for (int i = 0; i < scheduleVM.WorkPeriods.Count; i++)
                        {
                            string thisWorkType = scheduleVM.WorkPeriods[i].WorkType;
                            bool foundWorkType = false;
                            for (int j = 0; j < listLength; j++)
                            {
                                string thisWorkTypeValue = ViewBag.WorkTypeList[j].Text;
                                if (thisWorkType == thisWorkTypeValue)
                                {
                                    foundWorkType = true;
                                    break;
                                }
                            }
                            if (foundWorkType == false && thisWorkType != "null")
                            {
                                ViewBag.WorkTypeList.Add(new SelectListItem { Text = thisWorkType, Value = thisWorkType });
                                listLength += 1;
                            }
                        }
                        return View("Create", newScheduleVM);

                    case "Save Schedule":
                        Schedule schedule = new Schedule(scheduleVM);
                        db.Schedules.Add(schedule);
                        try
                        {
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch (DataException)
                        {
                            ModelState.AddModelError("", "Unable to save changes.");
                            break;
                        }
                }
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", scheduleVM.UserId);
            return View(scheduleVM);
        }

        // GET: Employer/Schedule/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", schedule.UserId);
            return View(schedule);
        }

        // POST: Employer/Schedule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Notes,ScheduleStartDay,UserId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", schedule.UserId);
            return View(schedule);
        }

        // GET: Employer/Schedule/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Employer/Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}