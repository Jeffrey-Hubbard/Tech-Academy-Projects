        // GET: Calendar
        public ActionResult Index()
        {
            return View("~/Views/Shared/Calendar/_index.cshtml");
        }

        //Find all actions and send them to as a Json Object
        public ActionResult findAll()
        {
            //This is currently the hard coded method to get the calendar events to show
            //functionality on FullCalendar.

            Schedule schedule = db.Schedules.Where(g => g.UserId == "2c8e6984-2015-4fde-bf94-7af270f2fa72").FirstOrDefault();

            // Calendar helper requires start and end date - these likewise are hard coded for proof-of-concept
            DateTime now = DateTime.Now;
            var eventstoFullCalendar = Calendar.ScheduletoFullCalendar(schedule, new DateTime(now.Year, now.Month - 1, 1), new DateTime(now.Year, now.Month + 1, DateTime.DaysInMonth(now.Year, now.Month)));

            return this.Content(eventstoFullCalendar, "application/json");

        }