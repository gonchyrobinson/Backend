# Copilot Chat Toolsets for Backend API (.NET 8, EF Core, MySQL)

Use these prompts in Copilot Chat to quickly scaffold new components that follow this project's conventions. Replace `Entidad` with your entity name and specify properties as needed.

---

## 1. Repository & Interface
**Prompt:**
> Create a repository and interface for the entity `Entidad` using the generic repository pattern. Place files in `Repositories/` and `Interfaces/`. Register the repository in `Program.cs`.

## 2. Service
**Prompt:**
> Generate a service class for `Entidad` that uses the corresponding repository and AutoMapper. Place it in `Services/` and register it in `Program.cs`.

## 3. DTO
**Prompt:**
> Create a DTO for `Entidad` with properties: [list your properties]. Place it in `DTOs/`.

## 4. Controller
**Prompt:**
> Generate an API controller for `Entidad` using the service and DTOs. Place it in `Controllers/`. Use dependency injection and follow the existing controller patterns.

## 5. Full CRUD Toolset
**Prompt:**
> Generate all files (interface, repository, service, DTO, controller) for a new entity `Entidad` with properties: [list your properties]. Register all dependencies in `Program.cs` and add an AutoMapper profile if needed.

---

**How to use:**
- Copy a prompt into Copilot Chat and provide your entity details.
- The generated code will match the architecture and patterns described in `.github/copilot-instructions.md` and `README.md`.
- Review and adjust as needed for custom logic or validation.

For more details on project structure and conventions, see `.github/copilot-instructions.md`.
