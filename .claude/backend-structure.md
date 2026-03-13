# Backend Structure

## Architecture Pattern

The backend follows **Clean Layered Architecture** with strict dependency flow:

```
Domain (innermost, no dependencies)
  ↑
Application (business logic, DTOs, interfaces)
  ↑
Infrastructure (EF Core, service implementations)
  ↑
Api (entry point, controllers, middleware)
```

## Project Breakdown

### 1. CodeCrafters.Domain

**Path:** `CodeCrafters.BackEnd/CodeCrafters.Domain/`

Pure domain entities with zero framework dependencies.

```
CodeCrafters.Domain/
└── Entities/
    └── SampleEntity.cs       # Example entity: Id (int), Name (string)
```

- No NuGet references — intentionally framework-agnostic.
- All domain entities go here.

---

### 2. CodeCrafters.Application

**Path:** `CodeCrafters.BackEnd/CodeCrafters.Application/`

Business logic layer containing DTOs, service interfaces, and orchestration.

```
CodeCrafters.Application/
├── DependencyInjection/
│   └── ApplicationServiceRegistration.cs   # DI extension method
└── HealthCheck/
    ├── Dtos/
    │   └── HealthCheckResponseDto.cs       # IsHealthy, DatabaseConnected, UtcTime
    └── Services/
        └── IHealthCheckService.cs          # Interface for health check
```

**Convention:** Each feature gets its own folder with `Dtos/` and `Services/` subfolders.

**References:** `CodeCrafters.Domain`

---

### 3. CodeCrafters.Infrastructure

**Path:** `CodeCrafters.BackEnd/CodeCrafters.Infrastructure/`

Data access, EF Core context, service implementations, and migrations.

```
CodeCrafters.Infrastructure/
├── Data/
│   └── AppDbContext.cs                     # EF Core DbContext
├── DependencyInjection/
│   └── InfrastructureServiceRegistration.cs # DI: DbContext + services
├── HealthCheck/
│   └── HealthCheckService.cs               # Implements IHealthCheckService
└── Migrations/
    ├── 20260313071347_InitialCreate.cs
    ├── 20260313071347_InitialCreate.Designer.cs
    └── AppDbContextModelSnapshot.cs
```

**Key Details:**
- `AppDbContext` contains `DbSet<SampleEntity> SampleEntities`
- Connection string resolution order:
  1. `"codecraftersdb"` from configuration
  2. `"DefaultConnection"` from configuration
  3. Fallback: `"Server=localhost;Database=CodeCraftersDb;Trusted_Connection=True;TrustServerCertificate=True;"`
- `HealthCheckService` checks DB connectivity via `dbContext.Database.CanConnectAsync()`

**References:** `CodeCrafters.Application`, `CodeCrafters.Domain`

---

### 4. CodeCrafters.Api

**Path:** `CodeCrafters.BackEnd/CodeCrafters.Api/`

ASP.NET Core Web API entry point.

```
CodeCrafters.Api/
├── Controllers/
│   └── HealthCheckController.cs    # GET /api/healthcheck
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs  # Global 500 error handler with trace ID
├── Program.cs                      # App startup, DI, middleware pipeline
├── appsettings.json
├── appsettings.Development.json
├── Properties/
│   └── launchSettings.json         # http:5161, https:7031
└── Dockerfile                      # Multi-stage .NET 9 build
```

**Program.cs Pipeline:**
1. `AddServiceDefaults()` — Aspire observability
2. `AddApplicationServices()` — Application layer DI
3. `AddInfrastructureServices()` — Infrastructure layer DI
4. Swagger (development only)
5. `ExceptionHandlingMiddleware` — global error handler
6. `MapControllers()` + `MapDefaultEndpoints()`
7. Auto-migration on startup (`dbContext.Database.Migrate()`)

**References:** `CodeCrafters.Application`, `CodeCrafters.Infrastructure`, `CodeCrafters.ServiceDefaults`

---

## CodeCrafters.ServiceDefaults

**Path:** `CodeCrafters.ServiceDefaults/`

Shared cross-cutting configuration used by all services.

- OpenTelemetry: logging, metrics, tracing
- Service discovery integration
- HTTP resilience (retries/timeouts)
- Health check endpoint at `/health` (dev only)

---

## Adding a New Feature (Pattern)

1. **Domain:** Add entity in `Domain/Entities/`
2. **Application:** Create feature folder with `Dtos/` and `Services/` (interface)
3. **Infrastructure:** Implement service, add `DbSet` to `AppDbContext`, create migration
4. **Api:** Add controller in `Controllers/`
5. **DI:** Register services in respective `ServiceRegistration` classes
