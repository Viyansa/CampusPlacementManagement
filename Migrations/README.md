# Migrations

This folder is intentionally left for EF Core generated migration files.

After restoring/building the project, run:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

The application also calls `Database.MigrateAsync()` at startup, so generated migrations are applied automatically when the API starts.
