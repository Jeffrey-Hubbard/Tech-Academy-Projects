The initial implementation of the Schedule Create page was to generate the intial schedule for a new user right after the user was created by an administrator.

This page made extensive use of Razor syntax and especially HTML Helpers to create and populate the various inputs, whether from default data (the start schedule day is initially set to the hire date entered by the administrator) or from user entry. It also made Model Binding to the ViewModel created on form submission very easy.

The ViewModel on construction has a default number of initial WorkPeriods (7) and an default maximum schedule length (14), which are used to create the series of WorkPeriod input rows. Updating the number of rows either by changing the schedule length from a drop-down menu or by clicking the 'add one more row' button at the bottom reloads the page with the new number of WorkPeriods, keeping existing user entries through Model Binding and passing the ViewModel back to the view.

I also made extensive use of an external library that extends the jQuery Datepicker to include time, which we needed to track the schedules. The .cshtml page shows my JavaScript and jQuery work to restrain the user input to expected ranges, to ensure the schedule start date came before the schedule end date, and limit the available dates within the WorkPeriod picker (while also including the timepicker).

The Worktype list is a dropdownlist dynamically created based on a database query for existing Worktypes, while also giving an opportunity to add a new Worktype. Choosing "Add New Worktype" displays a modal window where you can add a new work type, which is then set for the WorkPeriod you have selected and added as an option for the dropdown list on all other WorkPeriods.
