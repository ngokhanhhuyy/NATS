# NATS LANDING PAGE
## A landing page with admin tool behind for content edition

**Summary**
This project aims at creating a landing page which has a effective tool as administrator and content editor behind.
The administrator pages have been designed and structured in the way that makes the user, even a person that doesn't use a computer a lot, feel easy to interact with, including responsiveness, optimized for both desktop and mobile screens.

**Additional features**
- Recording traffic data (such as visits, views and devices), analyzing and visualizing as charts.
- Receving inquries from visitor from form.

**Things learned**
- Diving deeper into the web framework, exploring and solving some common HTTP based traditional multiple-page-application.
- Effectively structure the code in different layers, following strictly the Separation-of-Concern design principle with MVC pattern.
- Gaining some basic knowledge of DDD principles, strongly understanding the definition of Entities, Domain Models, DTO, Domain Layer, ...
- Gaining knowledge and experience of designing a basic database schema with complex relationships (One-To-One, One-To-Many, Many-To-Many).
- Diving deeper into database interaction with EntityFramework, learning how to write effective and optimize LINQ query, gaining strong knowledge of database construction with Code-First approach using FluentAPI.
- Ganing the basic knowledge of Javascript, reactivity in client-side, interaction between client-side and server-side through AJAX requests.
- Ganing the strong knowledge of Authentication and Authorization merchanism, with ASP.NET Identity.
- Ganing basic releasing and deployment process knowledge (using a shared hosting).

**Story you may interest**
> During the development of the creating, updating and deleting features, I have found that a complex data structure which contains a collections of files, submitted from a list of HTML file input elements, if facing some errors (such as validation error or business logic violation), the files cannot
be shipped back to the client via view models (as it is with other data types, like text). This problem, initially, is not that critical, but the target of the application is to make it as easy-to-use as possible, even without any manual document. So I have to design and create a whole new reactivity
merchanism with Javascript, starting from the states of the input elements (such as buttons to interact with a file input), and then a merchanism to send ajax requests and receive responses, populate the response data from the requests with Javascript instead of using Razor for rendering in the server side.
The AJAX feature was later decided to be used everywhere, even not needed, for consistency, so I tried to create an abstraction layer so that it can be used more effectively. That layer first took me a lot of effort and time to design and redesign over and over. And then when it works, the productivity for
implementing other CRUD operation requests in the other places has been boosted insanely.
This is the opening step that help me to step my foot into the front-end world, with VueJS library, to make a complex and completely Single Page Application in the next project.

**Libraries and frameworks used**
- Backend: ASP.NET Core MVC (.NET 7) with Razor engine, Entity Framework Core, FluentValidation, MagickImage, MySQL 8.
- Frontend: Bootstrap 5.3, Vanilla JavaScript for user interface reactivity and AJAX requests, ApexCharts for Admin dashboard statistics visualization.
- Github (source code management).
- IDE: Visual Studio Code (Frontend), Visual Studio 2022 (Backend and publishing), Rider (partially, for code optimization suggesstion), DataGrip (for direct database interaction and query testing).

**Some actual screenshot**

![Homepage](https://i.ibb.co/jJH5JNq/Homepage.png)

![Admin dashboard](https://i.ibb.co/KrV9Y8k/Admin-Dashboard.png)

![A page for editing content](https://i.ibb.co/Lt4BX2c/Admin-CRUD.png)
