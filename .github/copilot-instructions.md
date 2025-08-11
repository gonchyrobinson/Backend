# Copilot Instructions for Backend API (.NET 8, EF Core, MySQL)

## Project Architecture
- **Backend/Backend/**: Main source code. Key folders:
  - `Controllers/`: API endpoints, thin controllers, use dependency injection.
  - `Services/`: Business logic, injected into controllers.
  - `Repositories/`: Data access, use EF Core, injected into services.
  - `DTOs/`: Data transfer objects for input/output.
  - `Interfaces/`: Service and repository contracts.
  - `Contexts/`: `ApplicationDbContext` for EF Core.
  - `Mappings/`: AutoMapper profiles.
  - `Exceptions/`: Custom exception types.
  - `Constants/`: App-wide constants.
- **Backend.Tests/**: Unit tests (run with `dotnet test`).
- **docker/**, **database/**: Docker and DB setup/configuration.

## Key Patterns & Conventions
- **Dependency Injection**: All services and repositories are registered in `Program.cs` using `AddScoped`. Use constructor injection everywhere.
- **Repository Pattern**: Generic `Repository<T>` implements `IRepository<T>`. Custom repositories (e.g., `RepositorioPasantias`) extend or wrap this.
- **Service Pattern**: Business logic in services (e.g., `ServicioPagos`). Services use repositories, not DbContext directly.
- **DTO Usage**: All controller input/output uses DTOs, not EF entities.
- **Exception Handling**: Use custom exceptions (`AppException`, `ValidationException`) and global middleware (`ExceptionMiddleware`).
- **AutoMapper**: All entity/DTO mapping via AutoMapper. Profiles in `Mappings/`.
- **JWT Auth**: All protected endpoints use `[Authorize]`. JWT config in `appsettings.json`.
- **CORS**: Configured for local dev ports and production. See `Program.cs` for allowed origins.

## Developer Workflows
- **Build**: `dotnet build` or `./manage.ps1 local`
- **Run Locally**: `./manage.ps1 local` (runs API, DB, etc.)
- **Start Full Stack (Docker)**: `./manage.ps1 start`
- **Setup DB**: `./manage.ps1 setup`
- **View Logs**: `./manage.ps1 logs`
- **Run Tests**: `cd Backend.Tests; dotnet test`
- **Swagger UI**: http://localhost:5000/swagger

## Integration Points
- **Database**: MySQL, connection string in `appsettings.json` and `.env`.
- **Docker**: Compose files in `docker/` for dev/prod.
- **Frontend**: CORS allows localhost ports for React/Vite dev servers.

## Examples
- **Controller**: See `Controllers/StudentsController.cs` for DI and DTO usage.
- **Service**: See `Services/ServicioPagos.cs` for business logic and validation.
- **Repository**: See `Repositories/Repository.cs` for generic CRUD.

## Special Notes
- **Logical Delete**: Entities with `Eliminado`/`FechaEliminacion` use logical delete in repositories.
- **Error Logging**: Serilog is configured in `Program.cs`.
- **Database Initialization**: On startup, DB is auto-created if missing (see `Program.cs`).

---
For more, see `README.md` and `Program.cs`.
