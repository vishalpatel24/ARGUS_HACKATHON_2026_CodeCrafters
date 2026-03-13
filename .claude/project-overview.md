# Project Overview

## What Is This Project?

**CodeCrafters** is a full-stack web application built for the ARGUS Hackathon 2026. It follows enterprise-grade architectural patterns and is designed for rapid feature development. The project is a clean, scaffolded foundation ready for domain-specific feature implementation.

## Main Technologies

| Layer | Technology |
|-------|-----------|
| Backend | .NET 9, ASP.NET Core Web API |
| ORM | Entity Framework Core 9 (Code-First) |
| Database | SQL Server |
| Frontend | Angular 21.2 (Module-Based) |
| Orchestration | .NET Aspire 13.1.2 |
| Observability | OpenTelemetry |
| Containerization | Docker (multi-stage builds) |

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                   .NET Aspire AppHost (Orchestrator)            │
│                                                                 │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────────┐  │
│  │   SQL Server │    │  .NET 9 API  │    │  Angular 21 UI   │  │
│  │  (port 1433) │◄───│  (port 6700) │◄───│   (port 4300)    │  │
│  └──────────────┘    └──────────────┘    └──────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

### Request Flow

```
Browser → Angular UI (:4300)
           ↓ /api/* (proxy or direct)
        ASP.NET Core API (:6700 / :5000 internal)
           ↓ EF Core
        SQL Server (:1433)
```

## Project Structure

```
ARGUS_HACKATHON_2026_CodeCrafters/
├── CodeCrafters.Aspire.AppHost/     # Aspire orchestration host
├── CodeCrafters.BackEnd/
│   ├── CodeCrafters.Api/            # Web API entry point
│   ├── CodeCrafters.Application/    # Business logic + DTOs
│   ├── CodeCrafters.Domain/         # Domain entities
│   └── CodeCrafters.Infrastructure/ # EF Core, repositories, services
├── CodeCrafters.FrontEnd/
│   └── codecrafters-ui/             # Angular SPA
├── CodeCrafters.ServiceDefaults/    # Shared observability config
└── global.json                      # .NET SDK 9.0.100 pin
```

## Solution File

- `ARGUS_HACKATHON_2026_CodeCrafters.sln` — Visual Studio solution
- `CodeCrafters.slnx` — VS Code workspace

## Current State

The project is a scaffolded hackathon starter with:
- Clean architecture backend with one sample entity
- Angular frontend with routing and HTTP interceptor wiring
- Docker and Aspire orchestration fully configured
- Database migrations applied (InitialCreate)
- Health check endpoint working end-to-end
