backend/
├── Api/                                # API layer – handles HTTP requests and routes
│   ├── Controllers/                    # RESTful endpoints grouped by domain (e.g., Auth, Users)
│   ├── Common/                         # Standard API response formatting (ApiResponse<T>)
│   ├── Filters/                        # Custom attributes for authentication & RBAC (e.g., [HasPermission])
│   ├── Dtos/                           # Data Transfer Objects (LoginDto, UserDto, etc.)
│   ├── Program.cs                      # Entry point of the ASP.NET Core application
│   └── appsettings.json                # Configuration (Database, JWT, etc.)

├── Infrastructure/                     # Core logic, data access, and service layer
│   ├── Entities/                       # EF Core entities mapped to DB (User, Role, Permission, etc.)
│   ├── Repositories/                   # Repository pattern (GenericRepository, IRepository, etc.)
│   ├── Services/                       # Business logic (e.g., AuthService, UserService)
│   ├── Data/                           # EF Core DbContext and database setup
│   ├── Extensions/                     # Dependency injection and service registration
│   └── Infrastructure.csproj           # Project file for Infrastructure

├── backend.sln                         # Visual Studio Solution file
