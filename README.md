# LoginSolution

Beginner-friendly ASP.NET Core banking login starter solution using .NET 7, ASP.NET Core MVC frontends, ASP.NET Core Web API backends, EF Core 7, SQL Server Express, SQL Authentication, JWT bearer tokens, repository pattern, Razor views, Bootstrap, and MVC-to-API `HttpClient` communication.

## Architecture overview

```text
MVC projects -> Shared.Domain only
MVC projects -> APIs through HttpClient
API projects -> Shared.Domain + Shared.Database
Shared.Database -> Shared.Domain
Shared.Domain -> no database dependency
```

Projects:

- `src/IBanking/LoginSolution.IBanking.Mvc`: customer-facing MVC app on `https://localhost:7100`.
- `src/IBanking/LoginSolution.IBanking.Api`: customer API on `https://localhost:7101`; this is the only migration/seeding host.
- `src/Admin/LoginSolution.Admin.Mvc`: admin MVC app on `https://localhost:7200`.
- `src/Admin/LoginSolution.Admin.Api`: admin API on `https://localhost:7201`.
- `src/Shared/LoginSolution.Shared.Domain`: request/response models, constants, exceptions, interfaces, filters.
- `src/Shared/LoginSolution.Shared.Database`: EF Core entities, `LoginDbContext`, configurations, repositories, migrations, seeder.

MVC projects do not reference `Shared.Database`, do not contain SQL connection strings, and do not use EF Core. They store the API JWT server-side in session and use authentication cookies for page protection.

## Prerequisites

- .NET 7 SDK/runtime support. This solution targets `net7.0` in every project.
- SQL Server Express, expected instance: `.\SQLEXPRESS`.
- SQL Server Management Studio (SSMS).
- SQL Server and Windows Authentication mode, also called Mixed Mode Authentication.

## SQL Server Express setup

In SSMS, enable Mixed Mode Authentication:

1. Connect to `.\SQLEXPRESS`.
2. Right-click the server, open Properties, then Security.
3. Select `SQL Server and Windows Authentication mode`.
4. Restart the SQL Server Express service.

Create the local development database/login:

```sql
USE master;
GO

IF DB_ID('LoginSolutionDb') IS NULL
BEGIN
    CREATE DATABASE LoginSolutionDb;
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.server_principals
    WHERE name = 'LoginSolutionApp'
)
BEGIN
    CREATE LOGIN LoginSolutionApp
    WITH PASSWORD = 'Replace-With-A-Strong-Development-Password';
END;
GO

USE LoginSolutionDb;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = 'LoginSolutionApp'
)
BEGIN
    CREATE USER LoginSolutionApp
    FOR LOGIN LoginSolutionApp;
END;
GO

ALTER ROLE db_datareader ADD MEMBER LoginSolutionApp;
ALTER ROLE db_datawriter ADD MEMBER LoginSolutionApp;
GO
```

For local development migrations only:

```sql
USE LoginSolutionDb;
GO

ALTER ROLE db_owner ADD MEMBER LoginSolutionApp;
GO
```

`db_owner` is for local development only. Use least-privilege permissions outside local development.

## User secrets

Application login credentials are used by Admin and Customer users. SQL Server login credentials are used by APIs to connect to SQL Server Express. These are separate credentials.

Initialize secrets:

```powershell
dotnet user-secrets init --project src/IBanking/LoginSolution.IBanking.Api
dotnet user-secrets init --project src/Admin/LoginSolution.Admin.Api
```

Set the same SQL connection string for both APIs:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLEXPRESS;Database=LoginSolutionDb;User Id=LoginSolutionApp;Password=YOUR_STRONG_PASSWORD;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true" --project src/IBanking/LoginSolution.IBanking.Api

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLEXPRESS;Database=LoginSolutionDb;User Id=LoginSolutionApp;Password=YOUR_STRONG_PASSWORD;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true" --project src/Admin/LoginSolution.Admin.Api
```

Set JWT secrets:

```powershell
dotnet user-secrets set "JwtSettings:SecretKey" "REPLACE_WITH_A_LONG_RANDOM_DEVELOPMENT_KEY_AT_LEAST_32_CHARS" --project src/IBanking/LoginSolution.IBanking.Api

dotnet user-secrets set "JwtSettings:SecretKey" "REPLACE_WITH_A_LONG_RANDOM_DEVELOPMENT_KEY_AT_LEAST_32_CHARS" --project src/Admin/LoginSolution.Admin.Api
```

Do not commit real SQL passwords or JWT signing keys.

## Restore, build, migrations

```powershell
dotnet restore
dotnet build LoginSolution.sln
```

Create the initial migration from the IBanking API startup project:

```powershell
dotnet ef migrations add InitialCreate `
  --project src/Shared/LoginSolution.Shared.Database `
  --startup-project src/IBanking/LoginSolution.IBanking.Api `
  --output-dir Migrations
```

Apply the database migration:

```powershell
dotnet ef database update `
  --project src/Shared/LoginSolution.Shared.Database `
  --startup-project src/IBanking/LoginSolution.IBanking.Api
```

`IBanking.Api` also runs `Database.MigrateAsync()` and the idempotent `DevelopmentDataSeeder` only in Development.

## Running the apps

Start each app in a separate terminal:

```powershell
dotnet run --project src/IBanking/LoginSolution.IBanking.Api
dotnet run --project src/IBanking/LoginSolution.IBanking.Mvc
dotnet run --project src/Admin/LoginSolution.Admin.Api
dotnet run --project src/Admin/LoginSolution.Admin.Mvc
```

URLs:

- IBanking MVC: `https://localhost:7100`
- IBanking API Swagger: `https://localhost:7101/swagger`
- Admin MVC: `https://localhost:7200`
- Admin API Swagger: `https://localhost:7201/swagger`

## Demo accounts

Seeded automatically by `IBanking.Api` in Development after migrations.

Admin MVC:

- URL: `https://localhost:7200`
- Email: `admin@loginsolution.local`
- Password: `Admin@12345`

IBanking MVC:

- URL: `https://localhost:7100`
- Email: `customer@loginsolution.local`
- Password: `Customer@12345`
- Account number: `1000000001`
- Starting balance: `100000.00`

The MVC login pages show demo password hints only in Development.

## API endpoints

IBanking API:

- `POST /api/authentication/login`
- `POST /api/authentication/refresh-token`
- `POST /api/authentication/change-password`
- `GET /api/profile/me`
- `GET /api/account`
- `GET /api/account/balance`

Admin API:

- `POST /api/admin-authentication/login`
- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `PATCH /api/users/{id}/status`
- `GET /api/roles`
- `GET /api/roles/{id}`
- `POST /api/roles`
- `PUT /api/roles/{id}`

Admin user/role endpoints require an Admin JWT. Customer endpoints require a Customer JWT.

## Common SQL authentication errors

- `Login failed for user 'LoginSolutionApp'`: confirm the SQL login password in user-secrets and that the login exists at server level.
- `SQL Server Authentication is not enabled`: enable Mixed Mode Authentication and restart SQL Server Express.
- `Cannot open database requested by the login`: create `LoginSolutionDb` and map the login to a database user.
- `Cannot connect to .\SQLEXPRESS`: verify SQL Server Express is installed and the service is running.
- `The login does not have database permission`: grant `db_datareader` and `db_datawriter`; for migrations locally, grant `db_owner`.
- `Certificate chain is not trusted`: this starter uses `Encrypt=True;TrustServerCertificate=True` for local development.
- `A network-related or instance-specific error occurred`: confirm instance name, SQL Browser/service status, firewall, and local server configuration.
- `The instance of SQL Server you attempted to connect to requires encryption but this machine does not support it`: update SQL Server client components/SQL Server Express, confirm TLS support, or review local encryption policy. Keep production encryption enabled.
