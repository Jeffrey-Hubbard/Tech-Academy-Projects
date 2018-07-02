using ScheduleUsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScheduleUsers.Areas.Employer.ViewModels
{
    public class ScheduleViewModel
    {
        /// <summary>
        /// FK from Users table
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// User First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Notes on the User's schedule
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Nullable DateTime for start of this Schedule
        /// </summary>
        public DateTime? ScheduleStartDay { get; set; }
        /// <summary>
        /// Nullable DateTime for end of this schedule (if no end, schedule is either repeating or open ended)
        /// </summary>
        public DateTime? ScheduleEndDay { get; set; }
        /// <summary>
        /// Boolean for repeating Schedule. If Repeating, Schedule will assume to repeat the same sequence of 
        /// Work Periods once the end of the list has been reached.
        /// </summary>
        public bool Repeating { get; set; }
        /// <summary>
        /// Length of Schedule in Days/Work Periods. Not stored in database, only used for GUI/View
        /// </summary>
        public int ScheduleLength { get; set; }
        /// <summary>
        /// List of WorkPeriod Objects, including working days and days off for that Schedule.
        /// </summary>
        public List<WorkPeriod> WorkPeriods { get; set; }
        /// <summary>
        /// This is a cap for the max length of Schedule. This currently only affects the drop-down
        /// list on the Schedule Create View
        /// </summary>
        public int MaxScheduleLength { get; set; } = 14;
        /// <summary>
        /// Populates Schedule Create View dropdown list for Day Off boolean
        /// </summary>
        public SelectList IsDayOffList = new SelectList(new[] {
                new SelectListItem{ Text = "true", Value = "Yes" },
                new SelectListItem{ Text = "false", Value = "No"}
                }, "Text", "Value", "false");

        public ScheduleViewModel()
        {
        }

        public ScheduleViewModel(string Id)
        {      
            UserId = Id; // this Id is the Id passed from user creation
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Where(x => x.Id == Id).First();
                FirstName = user.FirstName;
                LastName = user.LastName;
                ScheduleStartDay = user.HireDate;
            }
            //Generate default work period length for initial schedule create (currently 7)
            WorkPeriods = new List<WorkPeriod>();
            for (int i = 0; i < 7; i++)
            {
                WorkPeriods.Add(new WorkPeriod());
                // set default starttime (DateTime) for each period
                WorkPeriods[i].StartTime = ScheduleStartDay.Value.AddDays(i);
            }
        }

        public ScheduleViewModel(Schedule schedule)
        {
            UserId = schedule.UserId;
            ScheduleStartDay = schedule.ScheduleStartDay;
            ScheduleEndDay = schedule.ScheduleEndDay;
            ScheduleLength = ScheduleLength;
            Notes = schedule.Notes;
            Repeating = schedule.Repeating;
            WorkPeriods = schedule.WorkPeriods.ToList();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Where(x => x.Id == schedule.Id).First();
                FirstName = user.FirstName;
                LastName = user.LastName;
            }
        }

    }
}