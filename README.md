📁 Backend Architecture Overview
The backend of this Fintech application is structured into two main layers: Api/ and Infrastructure/, following a clean, scalable, and modular design pattern.

🔹 Api/ – API Layer
  This layer is responsible for exposing HTTP endpoints, handling client requests, and managing request pipelines. It includes:
  
  Controllers/ – Contains RESTful controllers for each domain (e.g., authentication, user management). Each controller defines endpoints and delegates processing to the service layer.
  
  Common/ – Houses shared structures for consistent API responses, like a generic ApiResponse<T> class.
  
  Filters/ – Contains custom authorization filters and attributes, such as [HasPermission], which enforce RBAC rules at the controller level.
  
  Dtos/ – Data Transfer Objects used to define request and response schemas, keeping API contracts clean and decoupled from entities.
  
  Program.cs – The application’s entry point that sets up the web host, middleware, and routing.
  
  appsettings.json – Configuration file for database connection strings, JWT settings, and other environment-based options.

🔹 Infrastructure/ – Core Logic & Data Layer
  This layer implements the core application logic and manages data access, encapsulating all business concerns. It includes:
  
  Entities/ – Defines EF Core entity models that represent database tables, such as User, Role, and Permission.
  
  Repositories/ – Implements the repository pattern with both generic and specific repositories, enabling clean data access and separation of concerns.
  
  Services/ – Contains the business logic of the application. For instance, the AuthService handles login, token generation, and permission checks.
  
  Data/ – Defines the EF Core DbContext, seed data, and database initialization logic.
  
  Extensions/ – Provides extension methods for configuring dependency injection and other startup services.
  
  Infrastructure.csproj – The project file for the infrastructure layer, responsible for compiling all core logic.

🔸 backend.sln – Solution File
The root solution file ties together the Api and Infrastructure projects, enabling a clean separation of application concerns while maintaining a unified build and deployment process.
