# API Architecture

## Overview

The API is an ASP.NET Core Web API following RESTful conventions. Controllers live in `CodeCrafters.Api/Controllers/` and are auto-discovered via `MapControllers()`.

## Base URL

| Environment | URL |
|-------------|-----|
| Local (direct) | `http://localhost:5161` or `https://localhost:7031` |
| Docker/Aspire | `http://localhost:6700` (external) → port `5000` (container) |
| Frontend proxy | `/api` → `http://localhost:5000` |

## API Endpoints

### Health Check

| Method | Route | Controller | Description |
|--------|-------|-----------|-------------|
| GET | `/api/healthcheck` | `HealthCheckController` | Returns system health status |

**Response:** `HealthCheckResponseDto`
```json
{
  "isHealthy": true,
  "databaseConnected": true,
  "utcTime": "2026-03-13T07:00:00Z"
}
```

### Aspire Health (ServiceDefaults)

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/health` | Built-in liveness check (dev only) |

## Request Flow

```
Angular UI
  ↓ HTTP request to /api/*
API Interceptor (adds headers: Content-Type, Accept)
  ↓
Proxy (dev) or direct (prod)
  ↓
ASP.NET Core Pipeline
  ↓
ExceptionHandlingMiddleware (catches unhandled errors → 500 + traceId)
  ↓
Controller
  ↓
Application Service (interface)
  ↓
Infrastructure Service (implementation)
  ↓
EF Core → SQL Server
```

## Error Handling

### ExceptionHandlingMiddleware

Located at `CodeCrafters.Api/Middleware/ExceptionHandlingMiddleware.cs`.

Catches all unhandled exceptions and returns:
```json
{
  "error": "An unexpected error occurred.",
  "traceId": "<Activity.Current.Id>"
}
```
- Status code: `500 Internal Server Error`
- Logs the exception with full details

## Controller Conventions

- Route prefix: `[Route("api/[controller]")]`
- Attribute: `[ApiController]`
- Inject services via constructor DI
- Return `IActionResult` or typed results

## Frontend-Backend Communication

- **Development:** Angular dev server proxies `/api/*` to `http://localhost:5000`
- **Production/Docker:** Both containers run behind Aspire; frontend reaches API via configured endpoint
- **Headers:** Interceptor sets `Content-Type: application/json` and `Accept: application/json`
- **HTTP client:** `ApiService` wraps Angular `HttpClient` with generic `get/post/put/delete` methods

## Adding a New Endpoint

1. Create service interface in `Application/{Feature}/Services/I{Feature}Service.cs`
2. Create DTOs in `Application/{Feature}/Dtos/`
3. Implement service in `Infrastructure/{Feature}/{Feature}Service.cs`
4. Register service in `InfrastructureServiceRegistration.cs`
5. Create controller in `Api/Controllers/{Feature}Controller.cs`
6. Use constructor injection to access the service
