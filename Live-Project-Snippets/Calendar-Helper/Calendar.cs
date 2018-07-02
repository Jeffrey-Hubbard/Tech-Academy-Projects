using ScheduleUsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ScheduleUsers.Helpers
{
    public static class Calendar
    {
        // Method will accept lists of Events or any class that inherits from Event
        public static string EventstoFullCalendar<T>(List<T> events)
        {
            var list = new List<CalendarEvent>();

            foreach (var eachEventType in events)
            {
                var eachEvent = eachEventType as Event;

                list.Add(new CalendarEvent
                {
                    id = eachEvent.Id,
                    title = eachEvent.Title,
                    description = eachEvent.Note,
                    start = eachEvent.Start,
                    end = eachEvent.End,
                    color = "purple"
                });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string eventstoFullCalendar = serializer.Serialize(list);

            return eventstoFullCalendar;
        }

        public static string ScheduletoFullCalendar(Schedule schedule, DateTime startDate, DateTime endDate)
        {
            var eventList = new List<CalendarEvent>();
            // pull the list of workperiods and reorder them by start date ascending
            var sortedWorkPeriods = schedule.WorkPeriods;
            sortedWorkPeriods = sortedWorkPeriods.OrderBy(x => x.StartTime).ToList();
            // parse schedule and create events, adding them to eventList
            for (DateTime runningDate=startDate.Date; runningDate <= endDate.Date; runningDate += TimeSpan.FromDays(1))
            {
                if (runningDate >= schedule.ScheduleStartDay && 
                    (runningDate <= schedule.ScheduleEndDay || 
                    (schedule.ScheduleEndDay == null && 
                    ((runningDate - schedule.ScheduleStartDay).Days < schedule.WorkPeriods.Count || schedule.Repeating))))
                {
                    int scheduleIndex = (runningDate - schedule.ScheduleStartDay).Days % schedule.WorkPeriods.Count();
                    int scheduleCycleCount = Convert.ToInt32( Math.Floor(Convert.ToDouble((runningDate.Date - schedule.ScheduleStartDay).Days / schedule.WorkPeriods.Count)));
                    if ( sortedWorkPeriods.ElementAt(scheduleIndex).IsDayOff == false )
                    {
                        eventList.Add(new CalendarEvent
                        {
                            id = schedule.Id,
                            title = schedule.User.LastName,
                            description = schedule.Notes,
                            start = sortedWorkPeriods.ElementAt(scheduleIndex).StartTime.Value.AddDays(schedule.WorkPeriods.Count * scheduleCycleCount),
                            end = sortedWorkPeriods.ElementAt(scheduleIndex).EndTime.Value.AddDays(schedule.WorkPeriods.Count * scheduleCycleCount),
                            color = "red"
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string scheduletoFullCalendar = serializer.Serialize(eventList);
            // return string to original call
            return scheduletoFullCalendar;
        }
    }

    internal class CalendarEvent
    {
        /// <summary>
        /// This is the id passed to FullCalendar via Calendar helper. Name must be lower case to be read by FullCalendar.
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Title shown on FullCalendar. No Title prop on existing models. Name must be lower case to be read by FullCalendar.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Description shown on FullCalendar pop-up modal. Mapped to Notes. Name must be lower case to be read by FullCalendar.
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Event start time. TODO: FullCalendar shows with date offset but database is in local time. Name must be lower case to be read by FullCalendar.
        /// </summary>
        public DateTime? start { get; set; }
        /// <summary>
        /// Event end time. TODO: FullCalendar shows with date offset but database is in local time. Name must be lower case to be read by FullCalendar.
        /// </summary>
        public DateTime? end { get; set; }
        /// <summary>
        /// Color of event on FullCalendar
        /// </summary>
        public string color { get; set; }
    }
}