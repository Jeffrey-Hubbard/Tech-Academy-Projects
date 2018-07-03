Since our project needed to track time as well as date, the decision was made prior to my inclusion in the project to use an external library for a datetimepicker to extend the standard jQuery datepicker. When we needed only dates, we used the standard jQuery datepicker. I handled incorporation of these pickers in several locations, including schedule start and end dates (datepicker only) and work period start and end dates/times (datetimepicker).

In all cases, we sought to restrain the user's input to logical choices, for ease of use and to ensure the quality and integrity of the data submitted to the data base. This included:

1. Set default schedule start date to hire date set by administer during registration (this page was designed for the initial schedule creation of a new employee).
2. Use that schedule start date as the minimum date selectable in the datepicker, as a schedule could not start prior to employment.
3. Set and dynamically change the end date picker minimum date to ensure it never occurs prior to the schedule start date.
4. Set the WorkPeriod start date to a static date (same minimum and maximum date) according to their order in the Schedule list and the Schedule start date (while still allowing time input).
4. Set the WorkPeriod end date to a 2 day range, including the same day as the start day and also the next day, to allow for overnight shifts (allowing full range of time selection).

Handling these dynamic changes required significant work in jQuery and JavaScript to set and change the limitations of each picker, especially the pickers that were generated dynamically based on the schedule length.

I learned much in this process about DOM object access and processing, separation of front- and back-end logic and processing, and also the limits of certain technologies. My initial implementation leaned heavily on jQuery access and commands, and I found it to be too slow. I re-wrote much of the logic to use standard JavaScript calls and sped up the initial site responsiveness by about 75%.
