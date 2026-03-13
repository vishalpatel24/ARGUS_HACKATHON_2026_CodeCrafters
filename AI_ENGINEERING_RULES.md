## AI Engineering Rules

This document defines project-wide rules for AI coding agents (Cursor, Antigravity agents, and other LLM-based assistants) working on this repository.

Agents must **read, respect, and follow** these rules before generating or modifying code.

---

## SECTION 1 — GENERAL PRINCIPLES

- **Working software first**: Prefer delivering working, testable features over theoretical perfection.
- **Keep it simple**: Avoid overengineering, unnecessary abstractions, or patterns that do not clearly add value.
- **SOLID**: Follow SOLID principles in class and service design where it improves clarity and extensibility.
- **Clean architecture layering**: Respect the dependency direction:
  - `Domain` → `Application` → `Infrastructure` → `API`
  - Inner layers must not depend on outer layers.
- **Readability and maintainability**: Prefer clear, boring, well-structured code over clever or overly concise implementations.
- **Consistency over novelty**: Follow existing patterns in the codebase unless they are clearly harmful.

---

## SECTION 2 — BACKEND RULES (.NET)

Target stack: **.NET 9 Web API**, **Entity Framework Core**, **SQL Server**, **Clean layered architecture**, orchestrated with **.NET Aspire**.

### 2.1 Architecture and Layering

Structure backend code according to these layers:

- **Domain layer**
  - Contains only **entities** and **core domain models/value objects**.
  - No infrastructure, no EF-specific types, no ASP.NET dependencies.
  - Domain logic that is pure and independent of external frameworks is allowed here.

- **Application layer**
  - Contains **DTOs**, **interfaces**, **application services**, and **use-case orchestration**.
  - Depends on `Domain`, but not on `Infrastructure` or `API`.
  - Contains business rules and workflows that use repositories and other abstractions.

- **Infrastructure layer**
  - Contains **EF Core `DbContext`**, **entity configurations**, **repository implementations**, and external integrations (e.g., email, queues, external APIs).
  - Implements interfaces defined in the `Application` layer.
  - Depends on `Domain` and `Application`, but not on `API`.

- **API layer**
  - Contains **controllers**, **middleware**, **filters**, and **API-specific models (if needed)**.
  - Depends on `Application` (and indirectly on `Domain` and `Infrastructure` via DI).
  - No direct database or infrastructure logic.

AI agents must **not violate these layer boundaries**. Controllers should only depend on application services/interfaces, never directly on `DbContext` or repository implementations.

### 2.2 Coding Standards

- **Async everywhere**
  - Prefer async methods across controllers, services, and repositories: use `async`/`await` and return `Task`/`Task<T>`.
  - Use EF Core async methods (e.g., `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`).

- **Dependency Injection**
  - All services, repositories, and DbContexts must be registered in the DI container.
  - Do not instantiate services, DbContexts, or HttpClients with `new` inside application or API code.

- **Controller responsibilities**
  - Controllers:
    - Accept and validate HTTP requests.
    - Call **application services only**.
    - Map between API models/DTOs and application DTOs if needed.
  - Controllers must **never access `DbContext` directly**.
  - Controllers must **not contain business logic** beyond simple orchestration and mapping.

- **Service responsibilities**
  - Application services contain the **business logic** and orchestrate repositories and domain operations.
  - Keep services cohesive and focused on a specific business capability or aggregate.

### 2.3 Entity Rules

- **Primary keys**
  - All entities must have an `Id` property as the primary key (typically `int`, `long`, or `Guid`).

- **Relationships**
  - Use **navigation properties** for relationships (e.g., `ICollection<TChild>` for collections, reference navigation for single relations).
  - Configure relationships explicitly via EF Core fluent configuration where needed (e.g., `OnModelCreating` or configuration classes).

- **Configurations**
  - Use EF Core configuration classes or `OnModelCreating` for:
    - Table names.
    - Keys and relationships.
    - Property constraints (length, required, etc.).
  - Avoid sprinkling data annotations if the project primarily uses fluent configuration; follow the existing style.

### 2.4 DTO Rules

- **Separate DTO types**
  - Define **separate DTOs** for:
    - **Create** requests (e.g., `CreateXyzDto`).
    - **Update** requests (e.g., `UpdateXyzDto`).
    - **Response** models (e.g., `XyzResponseDto`).
  - Do not reuse entity types as API DTOs.
  - Keep DTOs free of domain logic.

### 2.5 API Rules

- **REST conventions**
  - Endpoints must use standard REST naming:
    - `GET /api/resource`
    - `GET /api/resource/{id}`
    - `POST /api/resource`
    - `PUT /api/resource/{id}`
    - `DELETE /api/resource/{id}`
  - Use plural resource names where appropriate (e.g., `/api/users`, `/api/orders`).
  - Use route attributes consistently (e.g., `[Route("api/[controller]")]`).

- **Status codes**
  - Return appropriate HTTP status codes (e.g., `200/201/204` for success, `400` for validation errors, `404` when not found).

### 2.6 Validation

- **FluentValidation**
  - Use **FluentValidation** when input validation is needed.
  - Validation should:
    - Apply to input DTOs (create/update).
    - Integrate with ASP.NET Core validation pipeline where possible.
  - Do not duplicate validation rules across layers; keep them centralized in validators.

---

## SECTION 3 — DATABASE RULES

- **SQL Server conventions**
  - Design schemas compatible with SQL Server conventions (naming, data types, indexes).

- **Migrations**
  - Use **EF Core migrations** for all schema changes.
  - Do not manually edit the database schema outside migrations unless explicitly required and documented.

- **Indexes and keys**
  - Use proper indexing for **foreign keys** and frequently queried columns.
  - Avoid unbounded or unnecessary indexes that degrade write performance.

- **Schema design**
  - Avoid large, monolithic tables that mix many unrelated concerns.
  - Normalize where reasonable, but do not over-normalize for hackathon timelines.

---

## SECTION 4 — ANGULAR RULES

Frontend stack: **Angular** with **module-based architecture** (no default to standalone components).

### 4.1 Architecture and Folder Structure

Angular must use a **module-based architecture**, not standalone-only setups.

Required high-level folder structure:

- `src/app`
  - `core`
  - `shared`
  - `layout`
  - `features`

### 4.2 Core Module

- **Core module responsibilities**
  - Contains **singleton services** (e.g., auth, configuration, global app services).
  - Contains **HTTP interceptors** and other application-wide providers.
  - Imported **once** in the root module (`AppModule`) and never imported by feature modules.

### 4.3 Shared Module

- **Shared module responsibilities**
  - Contains **reusable components**, **pipes**, and **directives** used across multiple features.
  - Exports Angular Material modules and other shared UI modules if appropriate.
  - Does not contain singleton services that require a single instance.

### 4.4 Feature Modules

- **Per-feature modules**
  - Each feature must have its own module and routing module under `features`.

- **Feature structure**
  - `features/{feature}/`
    - `pages/` – page-level components (routed components).
    - `components/` – smaller, reusable feature-specific components.
    - `services/` – feature-specific services (e.g., API wrappers, state management for that feature).
    - `models/` – TypeScript interfaces/models for that feature.
    - `{feature}.module.ts` – feature module.
    - `{feature}-routing.module.ts` – routing configuration for the feature.

AI agents must follow this structure when generating new Angular features.

### 4.5 Component Rules

- **Reactive Forms**
  - Prefer **Reactive Forms** over Template-Driven Forms for new forms.

- **Services for API calls**
  - Components must **not call HTTP APIs directly**.
  - Use dedicated **services** (in `services/`) for API communication.
  - Components should subscribe to or use Observables returned from services.

- **Separation of concerns**
  - Keep components thin: mostly view logic and delegation to services.
  - Put business / decision logic into services or NgRx/etc. if such patterns already exist in the project.

---

## SECTION 5 — UI RULES

- **Angular Material first**
  - Prefer using **Angular Material components** for UI elements (inputs, buttons, dialogs, tables, etc.).

- **Clean and simple UI**
  - Keep interfaces uncluttered and easy to understand.
  - Use consistent spacing, typography, and colors (follow any existing design system if present).

- **CSS discipline**
  - Avoid unnecessary custom CSS when Angular Material already provides a good solution.
  - When custom styles are needed, scope them to components and avoid global overrides unless necessary.

---

## SECTION 6 — CODE GENERATION RULES

When generating **new features**, AI agents must ensure both backend and frontend elements are created together to keep the system consistent.

### 6.1 Backend Artifacts (per feature/entity)

Always generate:

- **Entity** (in `Domain`).
- **DTOs**:
  - Create DTO.
  - Update DTO.
  - Response DTO.
- **Service interface** (in `Application`).
- **Service implementation** (in `Infrastructure` or appropriate implementation layer, depending on existing project organization).
- **Controller** (in `API`) following the REST conventions in Section 2.5.

### 6.2 Frontend Artifacts (per feature)

Always generate:

- **Angular feature module** (`{feature}.module.ts`).
- **Feature routing module** (`{feature}-routing.module.ts`).
- **Feature service** for API communication.
- **List component** (page for listing entities, with pagination where appropriate).
- **Form component** (for create/update, using Reactive Forms).

Place these artifacts in the `features/{feature}` structure defined in Section 4.

---

## SECTION 7 — PERFORMANCE AND SAFETY

- **Pagination**
  - Use **pagination** (or suitable data windowing) for large lists, both at API and UI levels where applicable.

- **Async database calls**
  - Use async database calls (`SaveChangesAsync`, `ToListAsync`, etc.) to avoid blocking threads.

- **Avoid N+1 queries**
  - Use **`Include` / `ThenInclude`**, projection queries, or explicit loading to avoid N+1 query patterns.
  - Consider batching where appropriate.

- **Input validation and safety**
  - Always validate inputs (DTOs) using FluentValidation or built-in ASP.NET validation.
  - Do not trust client input; enforce constraints on the server.
  - Avoid exposing internal implementation details or sensitive data in responses.

---

## SECTION 8 — HACKATHON OPTIMIZATION

This project is developed during a hackathon. Optimize for **speed with reasonable structure**:

- **Simple implementations**
  - Prefer straightforward, easy-to-understand solutions over elaborate patterns.

- **Avoid unnecessary abstractions**
  - Do not add extra layers, generic frameworks, or over-generalized code unless they clearly solve a real need.

- **Working features over perfection**
  - Prioritize delivering working end-to-end features.
  - Architectural purity is secondary to delivering value, as long as the core rules in this document are respected.

---

## SECTION 9 — AI AGENT BEHAVIOR

AI coding agents working on this repository must:

- **Read and follow these rules**
  - Consult this document **before** generating or modifying code.

- **Respect the defined architecture**
  - Do not bypass or break the defined layers (Domain, Application, Infrastructure, API).
  - In Angular, respect the module-based architecture and folder structure.

- **Avoid unapproved architectural changes**
  - Do **not** introduce new major architectural patterns (e.g., switching to CQRS, event sourcing, or a new state management library) without a strong reason and explicit user request.

- **Modify existing code carefully**
  - Prefer minimal, targeted changes.
  - Preserve existing patterns and style unless they are clearly incorrect or harmful.

- **Keep changes minimal and consistent**
  - When extending functionality, follow the existing naming conventions, structure, and patterns in the relevant area.
  - When in doubt, choose the simplest option that still aligns with this document.

If any rule here conflicts with explicit, current user instructions, **user instructions take precedence**, but agents should call out the deviation briefly in their explanation.

