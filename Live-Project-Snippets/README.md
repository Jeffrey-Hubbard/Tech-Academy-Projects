Our Live Project is a web-based application for tracking schedules, time-off, and clock-ins and -outs.

The project is built as an ASP.NET MVC application, with development done in Visual Studio. Version Control and Backlog tickets/tasks were handled through a Team Foundation Services (TFS) server.

Development included Code-First database design with several migrations, C# (including Razor syntax), LINQ database queries, JavaScript, JQuery, HTML5, CSS3.

My focus in the project was to build and integrate the user schedule functions, including the controller and views to handle creation and saving of user schedules as well as the process for transforming both lists of Events (from the TimeOffEvent and WorkTimeEvent (aka clock-in and clock-out) to serialized JSON objects to be read and rendered by the calendar package FullCallendar.

Implementation direction was worked through during a Scrum, with a Scum Master generating tickets to track completion.

Upon completion of my tasks, I acted as Scrum Master for the next Sprint, writing up tickets after our Scrum planning session to assist the next cycle of developers keep their tasks organized and tracked.

Specifically, I learned and practiced:
1. Implementing UI features, from simple buttons that call controller actions to an entire View showing and requesting data that interfaces with the ViewModel and generating page elements dependant on ViewModel properties. I used datepickers and datetimepickers with dynamically changing restraints implemented with jQuery. I also implemented a pop-up modal window that will add a new option to the dropdown menu.
2. Changing scaffolded Models to account for changes from Agile development process since project start, including database migrations to handle new and changing data/datatypes.
3. Building a ViewModel to pass a specific set of Model properties (and other properties as needed) to the View, separating the View and the Model from each other.
4. Creating Controller methods, including GET and POST methods and methods with parameters, to control the program flow according to the design direction set by the team.
5. Using Model Binding and Razor Syntax with HTML Helpers to both pass ViewModel data to the View as well as return data to a Controller method by generating a ViewModel from those bound properties.
6. Handle data persistence, both between views via passing ViewModel data between Controller methods as well as saving data to the SQL database.
7. Generated a static class (Helper) with two methods to take in either a list of events or a schedule, start date, and end date, and generate serialized JSON data to pass to the FullCalendar library we will use for calendar display. The method that takes a schedule required logic to transform a schedule to a list of events, including handling schedules that repeat by iterating over needed dates and comparing them via index and offset to the 'virtual' schedule that would occupy that space, without needing to record or instantiate it.
