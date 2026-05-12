# Copilot Onboarding Instructions

Welcome to the WebAPIShop repository! This document provides high-level guidance for coding agents and contributors to work efficiently and consistently across the project.

---

## Project Summary

WebAPIShop is a modern e-commerce RESTful Web API built with .NET 9 and C#. It exposes endpoints for managing users, products, categories, orders, and ratings. The API is designed for maintainability, scalability, and high performance, following a layered architecture.

---

## Tech Stack

- **Framework:** .NET 9
- **Language:** C#
- **ORM:** Entity Framework Core (DB-First)
- **Mapping:** AutoMapper
- **Logging:** NLog
- **Testing:** xUnit, Moq, Moq.EntityFrameworkCore
- **Caching:** Redis (StackExchange.Redis)
- **API Documentation:** Swagger/OpenAPI (Swashbuckle)
- **Authentication:** JWT Bearer
- **Dependency Injection:** .NET built-in DI

---

## Project Structure

```text
├── WebAPIShop/          # Main API project (controllers, middleware, Program.cs)
├── Services/            # Business logic (service layer)
├── Repositories/        # Data access (repository layer)
├── Entities/            # Domain models (database entities)
├── DTOs/                # Data Transfer Objects (C# records)
├── TestProject1/        # xUnit test project (unit & integration tests)
├── appsettings.json     # Configuration (connection strings, JWT, Redis, etc.)
└── .github/             # Copilot and contribution instructions 
```


---

## Coding Guidelines

- **Layered Architecture:** Keep API, service, and repository layers separate.
- **Async Everywhere:** All data access and service methods must be asynchronous.
- **Dependency Injection:** Use constructor injection for all dependencies.
- **DTOs:** Use C# records for DTOs; do not expose entity models directly.
- **Error Handling:** Use global middleware for exception handling.
- **Logging:** Use NLog; avoid `Console.WriteLine`.
- **Testing:** All new features must include unit and, where appropriate, integration tests.
- **Configuration:** Store all environment-specific settings in `appsettings.json` or environment variables.
- **Swagger:** Document all public endpoints using XML comments for automatic Swagger generation.

---

## Build & Run

- **Restore dependencies:** `dotnet restore`
- **Apply migrations:** `dotnet ef database update`
- **Run API:** `dotnet run --project WebAPIShop`
- **Run tests:** `dotnet test`

---

## Tools & Resources

- **AutoMapper:** For mapping between entities and DTOs.
- **NLog:** For logging (configured in Program.cs and appsettings).
- **Swagger:** For API documentation and testing.
- **Redis:** For distributed caching.
- **xUnit & Moq:** For unit and integration testing.

---

## Additional Guidance

- **No direct database access in controllers or services.** All data access must go through repositories.
- **Follow RESTful conventions** for all API endpoints (see `.github/instructions/controllers.md`).
- **Use dependency injection** for all services and repositories (see `.github/instructions/repositories.md`).
- **Do not hardcode configuration values**; use `IConfiguration` and `appsettings.json`.
- **Check for existing DTOs and mapping profiles** before creating new ones.

For more details on repository and controller patterns, see the files in `.github/instructions/`.

---