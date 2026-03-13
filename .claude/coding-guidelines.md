# Coding Guidelines

## Backend (.NET)

### Architecture Rules

- **Clean Layered Architecture** — dependencies flow inward only:
  - `Domain` has no references
  - `Application` references only `Domain`
  - `Infrastructure` references `Application` and `Domain`
  - `Api` references `Application`, `Infrastructure`, and `ServiceDefaults`
- No cross-layer violations. Never reference `Infrastructure` from `Application`.

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `HealthCheckService` |
| Interfaces | I-prefix + PascalCase | `IHealthCheckService` |
| Methods | PascalCase | `CheckHealthAsync` |
| Properties | PascalCase | `IsHealthy` |
| Private fields | camelCase | `logger` |
| Async methods | Async suffix | `CheckHealthAsync` |
| DTOs | Suffix with Dto | `HealthCheckResponseDto` |
| Controllers | Suffix with Controller | `HealthCheckController` |

### Patterns

- **Dependency Injection:** All services registered via extension methods in `DependencyInjection/` folders
- **Service interfaces** defined in `Application`, implementations in `Infrastructure`
- **DTOs** are separate from domain entities — never expose entities directly in API responses
- **Feature folders:** Group related DTOs, services, and controllers by feature, not by type
- **Async/await:** All I/O-bound operations use async with `CancellationToken` support
- **Constructor injection:** Prefer over service locator pattern

### Error Handling

- Global exception middleware catches unhandled errors
- Return appropriate HTTP status codes from controllers
- Log exceptions with structured logging (`ILogger<T>`)

### Controller Rules

- Use `[ApiController]` and `[Route("api/[controller]")]` attributes
- Return `IActionResult` or typed `ActionResult<T>`
- Keep controllers thin — delegate logic to services

---

## Frontend (Angular)

### Architecture Rules

- **Module-based** architecture (NgModule, not standalone)
- Core services in `core/services/` — injectable at root
- Feature modules lazy-loaded via routing
- Interceptors in `core/interceptors/`

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Components | kebab-case files | `my-feature.component.ts` |
| Services | kebab-case + .service | `api.service.ts` |
| Modules | kebab-case + .module | `app-module.ts` |
| Interceptors | kebab-case + .interceptor | `api.interceptor.ts` |
| Interfaces | PascalCase, I-prefix optional | `HealthCheckResponse` |

### Patterns

- Use `ApiService` for all HTTP calls — do not use `HttpClient` directly in components
- Environment-specific config in `environments/` files
- SCSS for styling
- Angular signals for reactive state where applicable

### File Naming

This project uses a non-standard Angular file naming pattern (no `.component` suffix):
- `app.ts` instead of `app.component.ts`
- `app-module.ts` instead of `app.module.ts`
- `app-routing-module.ts` instead of `app-routing.module.ts`

---

## General

- **No secrets in code** — use environment variables or Aspire configuration
- **Code-First database** — always create migrations, never modify the database directly
- **Docker builds** must work — test Dockerfile changes locally
- Prefer concise, readable code over verbose documentation
