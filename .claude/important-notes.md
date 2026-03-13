# Important Notes

## For Developers & AI Assistants

### 1. Auto-Migration on Startup

The API automatically applies pending EF Core migrations when it starts (`Program.cs`). You do **not** need to run `dotnet ef database update` manually — just add a migration and restart the API.

### 2. Aspire Orchestration is the Primary Run Mode

The project is designed to run via `.NET Aspire`. Running `dotnet run` in the AppHost project spins up SQL Server, the API, and the frontend together with proper service discovery and connection strings.

### 3. Non-Standard Angular File Names

This project does **not** use the typical `.component.ts` / `.module.ts` suffixes:
- `app.ts` (not `app.component.ts`)
- `app-module.ts` (not `app.module.ts`)

Keep this convention when adding new files.

### 4. Frontend Runs ng serve in Docker

The frontend Dockerfile runs `ng serve` (dev server), not a production build. For production, this should be changed to build static files and serve via nginx or similar.

### 5. Proxy Configuration Matters

In local development, the Angular dev server proxies `/api/*` to `http://localhost:5000`. If the backend runs on a different port, update `proxy.conf.json`.

### 6. Connection String Priority

The infrastructure layer checks three sources for the DB connection string. When running via Aspire, the `"codecraftersdb"` key is injected automatically. When running standalone, ensure `"DefaultConnection"` is set in `appsettings.json` or the fallback localhost string will be used.

### 7. AI_ENGINEERING_RULES.md

The repository root contains `AI_ENGINEERING_RULES.md` which documents architecture rules and coding standards established by the team. Always check this file for the latest guidelines before making changes.

### 8. .NET SDK Version Pinned

`global.json` pins the SDK to `9.0.100`. Ensure this version is installed.

### 9. OpenTelemetry is Configured

The `ServiceDefaults` project configures OpenTelemetry for logging, metrics, and tracing across all services. Traces include Activity IDs that appear in error responses.

### 10. Current Project State

This is a **hackathon scaffold** — the core architecture is in place but only a health check feature exists. The project is ready for rapid feature development following the established patterns:
- Add entities → Add DTOs + service interfaces → Implement services → Add controllers
- Add Angular feature modules with lazy-loaded routes

### 11. Do Not Modify Existing Files

When working on documentation or analysis tasks, only create files inside `.claude/`. Never modify existing project source code unless explicitly implementing a feature or fix.
