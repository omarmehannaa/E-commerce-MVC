# E-Commerce MVC Web Application

A full-stack e-commerce web application built using ASP.NET MVC, following a layered architecture (Presentation, Business, Data Access, and Models).  

---

🧩 Features

- Product listing, search, and details  
- Add to cart, update quantity, and checkout flow (simplified)  
- Basic user registration & login (authentication)  
- CRUD operations for products (admin-style)  
- Validation on forms  
- Separation of concerns (Business, Data Access, Models layers)  

---

💻 Tech Stack

| Layer           | Technologies / Tools        |
|----------------|-------------------------------|
| Presentation   | ASP.NET MVC, Razor views       |
| Business Logic | C#, custom services / managers |
| Data Access     | Entity Framework (or similar ORM) / repository pattern |
| Models          | C# POCOs / ViewModels          |
| Others          | SQL Server (or any relational DB), LINQ, NuGet packages |

---

🏗 Architecture & Design

This project uses a "layered architecture":

1. Presentation (E-commerce.MVC) — handles HTTP requests, controllers, and views  
2. Business (E-commerce.Business) — core logic, service / manager classes  
3. Data Access (E-commerce.DataAccess) — repositories, context, data operations  
4. Models (E-commerce.Models) — DTOs, entity definitions, view models  
