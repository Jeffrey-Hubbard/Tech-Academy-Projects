using ScheduleUsers.Areas.Employer.ViewModels;
using ScheduleUsers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScheduleUsers.Models
{
    /// <summary>
    /// Individual schedules for employees
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Identify each employee schedule using a unique Schedule Id
        /// </summary>
        public string Id { get; set; }

        [Display(Name = "Notes")]
        //public string ApplicationUserId { get; set; }
        /// <summary>
        /// Notes for pertinent information regarding employee schedules
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Specify day the work period starts
        /// </summary>
        [Display(Name = "Schedule Start Day")]
        //[DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleStartDay { get; set; }

        /// <summary>
        /// Specify day the work period ends (nullable)
        /// </summary>
        [Display(Name = "Schedule End Day")]
        //[DisplayFormat(DataFormatString = "{dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ScheduleEndDay { get; set; }

        /// <summary>
        /// List of individual work periods for employee
        /// </summary>
        [Display(Name = "Work Periods")]
        public virtual ICollection<WorkPeriod> WorkPeriods { get; set; }

        /// <summary>
        /// Registered employee user account
        /// </summary>
        /// 
        public string UserId { get; set; }

        /// <summary>
        /// Flag for Schedule management to know if schedule repeats when reaches end of days
        /// </summary>
        public bool Repeating { get; set; }


        public virtual ApplicationUser User { get; set; }
        
        public Schedule()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public Schedule(ScheduleViewModel scheduleViewModel)
        {
            Id = Guid.NewGuid().ToString();
            UserId = scheduleViewModel.UserId;
            Notes = scheduleViewModel.Notes;
            ScheduleStartDay = scheduleViewModel.ScheduleStartDay.GetValueOrDefault(DateTime.Now);
            ScheduleEndDay = scheduleViewModel.ScheduleEndDay;
            Repeating = scheduleViewModel.Repeating;
            WorkPeriods = scheduleViewModel.WorkPeriods;
        }
    }
}