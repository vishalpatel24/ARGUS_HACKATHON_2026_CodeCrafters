# Database Structure

## Database Engine

- **Engine:** SQL Server
- **Database Name:** `CodeCraftersDb`
- **ORM:** Entity Framework Core 9.0.0 (Code-First)
- **Provider:** `Microsoft.EntityFrameworkCore.SqlServer`

## Connection String Resolution

The infrastructure layer resolves connection strings in this order (see `InfrastructureServiceRegistration.cs`):

1. `"codecraftersdb"` — Aspire-injected connection string
2. `"DefaultConnection"` — standard config key
3. Fallback: `"Server=localhost;Database=CodeCraftersDb;Trusted_Connection=True;TrustServerCertificate=True;"`

## DbContext

**Class:** `AppDbContext` (`Infrastructure/Data/AppDbContext.cs`)

```csharp
public class AppDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
    }
}
```

## Current Schema

### Table: SampleEntities

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity(1,1) |
| Name | nvarchar(max) | NOT NULL |

## Migrations

| Migration | Date | Description |
|-----------|------|-------------|
| `20260313071347_InitialCreate` | 2026-03-13 | Creates `SampleEntities` table |

**Migration files location:** `CodeCrafters.Infrastructure/Migrations/`

## Auto-Migration

The API applies pending migrations automatically on startup in `Program.cs`:

```csharp
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
dbContext.Database.Migrate();
```

## Aspire SQL Server Resource

Configured in `CodeCrafters.Aspire.AppHost/Program.cs`:

```
SQL Server resource: "sql" (port 1433)
Database resource: "codecraftersdb"
```

The API container receives the connection string via Aspire resource references.

## Adding a New Entity

1. Create entity class in `Domain/Entities/NewEntity.cs`
2. Add `DbSet<NewEntity>` to `AppDbContext`
3. Configure with Fluent API in `OnModelCreating()` if needed
4. Generate migration:
   ```bash
   dotnet ef migrations add MigrationName --project CodeCrafters.BackEnd/CodeCrafters.Infrastructure --startup-project CodeCrafters.BackEnd/CodeCrafters.Api
   ```
5. Migration will auto-apply on next startup
