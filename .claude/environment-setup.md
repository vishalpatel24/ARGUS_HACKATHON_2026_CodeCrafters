# Environment Setup

## Prerequisites

| Tool | Version | Purpose |
|------|---------|---------|
| .NET SDK | 9.0.100 | Backend build (pinned in `global.json`) |
| Node.js | 20+ | Frontend build |
| npm | 11.6+ | Frontend package management |
| SQL Server | Any recent | Database |
| Docker | Latest | Container builds |
| Angular CLI | 21.2.2 | Frontend dev tooling |

## Quick Start

### Option 1: Run with .NET Aspire (Recommended)

Aspire orchestrates all services (SQL Server, API, Frontend) together.

```bash
cd CodeCrafters.Aspire.AppHost
dotnet run
```

This starts:
- SQL Server on port `1433`
- Backend API on port `6700` (maps to container port `5000`)
- Frontend on port `4300` (maps to container port `4200`)

### Option 2: Run Backend Locally

```bash
# Ensure SQL Server is running locally
cd CodeCrafters.BackEnd/CodeCrafters.Api
dotnet run
```

API will be available at `http://localhost:5161` (or `https://localhost:7031`).

### Option 3: Run Frontend Locally

```bash
cd CodeCrafters.FrontEnd/codecrafters-ui
npm install
npm start
```

Frontend will open at `http://localhost:4200` with proxy forwarding `/api` to `http://localhost:5000`.

## Connection Strings

### Backend Database Connection

Resolved in order:
1. Aspire-injected: `"codecraftersdb"` (automatic when running via Aspire)
2. Config key: `"ConnectionStrings:DefaultConnection"` in `appsettings.json`
3. Fallback: `Server=localhost;Database=CodeCraftersDb;Trusted_Connection=True;TrustServerCertificate=True;`

### Frontend API URL

- Development: `/api` (proxied to `http://localhost:5000` via `proxy.conf.json`)
- Production: `/api` (expects server-side routing)

## Docker Builds

### Backend

```bash
# From solution root
docker build -f CodeCrafters.BackEnd/CodeCrafters.Api/Dockerfile -t codecrafters-api .
```

- Exposes port `5000`
- Multi-stage build (SDK → Runtime)

### Frontend

```bash
cd CodeCrafters.FrontEnd/codecrafters-ui
docker build -t codecrafters-frontend .
```

- Exposes port `4200`
- Runs `ng serve` inside container (dev mode)

## EF Core Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName \
  --project CodeCrafters.BackEnd/CodeCrafters.Infrastructure \
  --startup-project CodeCrafters.BackEnd/CodeCrafters.Api

# Apply migrations manually (also auto-applied on startup)
dotnet ef database update \
  --project CodeCrafters.BackEnd/CodeCrafters.Infrastructure \
  --startup-project CodeCrafters.BackEnd/CodeCrafters.Api
```

## Ports Summary

| Service | Local Port | Container Port |
|---------|-----------|---------------|
| SQL Server | 1433 | 1433 |
| Backend API (direct) | 5161 / 7031 | — |
| Backend API (Aspire) | 6700 | 5000 |
| Frontend (direct) | 4200 | — |
| Frontend (Aspire) | 4300 | 4200 |

## Swagger

Available in development mode at: `http://localhost:5161/swagger`
