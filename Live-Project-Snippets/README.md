Our Live Project is a web-based application for tracking schedules, time-off, and clock-ins and -outs.

The project is build as an MVC application, with development done in Visual Studio. Version Control and Backlog tickets/tasks were handled through a Team Foundation Services (TFS) server.

Development included Code-First database design with several migrations, C# (including Razor syntax), LINQ database queries, JavaScript, JQuery, HTML5, CSS3.

My focus in the project was to build and integrate the user schedule functions, including the controller and views to handle creation and saving of user schedules as well as the process for transforming both lists of Events (from the TimeOffEvent and WorkTimeEvent (aka clock-in and clock-out) to serialized JSON objects to be read and rendered by the calendar package FullCallendar.
