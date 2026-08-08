# CatchIQ

A fishing catch tracking app. Log catches with species, bait, weather, and location data, then use AI
analytics over your catch history to find patterns and catch more fish.

## Tech stack

**Backend:** .NET 10, ASP.NET Core Web API, EF Core 10, SQL Server, ASP.NET Identity + JWT, Swashbuckle

**Frontend:** React 19, TypeScript, Vite 8, Tailwind 4, React Router 7, NSwag

**Tooling:** GitHub Actions CI

## Architecture

### Controller → Manager → Engine → Accessor

| Layer | Responsibility |
| --- | --- |
| **Controller** | HTTP only: routing, model binding, status codes |
| **Manager** | Business rules, validation, orchestration across accessors |
| **Engine** | Reusable logic with no storage concerns (`TokenEngine` signs JWTs) |
| **Accessor** | EF Core data access |

Each layer sits behind an interface in DI. 

### Generated API client

The backend's OpenAPI document is fed to NSwag, which generates `src/api/generated.ts`: one typed client
per controller, with types matching the C# DTOs. 

Regenerate after changing a controller or DTO, with the backend running:

```bash
cd frontend && npm run generate-api
```

## Getting started

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download), [Node 22+](https://nodejs.org), and
[Docker](https://www.docker.com).

**1. Start SQL Server**

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=CatchIQ_Dev1!" \
  -p 1434:1433 --name catchiq-mssql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Later runs: `docker start catchiq-mssql`.

**2. Create `backend/appsettings.Development.json`** (gitignored; `appsettings.json` holds placeholders):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1434;Database=catchiq-mssql;User Id=sa;Password=CatchIQ_Dev1!;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Secret": "replace-with-a-long-random-signing-key",
    "Issuer": "CatchIQ",
    "Audience": "CatchIQUsers"
  }
}
```

**3. Apply migrations:**

```bash
cd backend && dotnet ef database update
```

**4. Set up the frontend env** (also gitignored; without it every API call hits the wrong URL):

```bash
cd frontend && cp .env.example .env
```

**5. Run both:**

```bash
cd backend && dotnet run --launch-profile http   # http://localhost:5045
cd frontend && npm install && npm run dev        # http://localhost:5173
```

## CI

`.github/workflows/ci.yml` builds both halves on every push and PR to `main`: `dotnet build -c Release`
against `CatchIQ.slnx`, and `npm ci` → lint → `npm run build` (which typechecks, unlike the dev server).
Client generation is excluded, since NSwag needs a running backend.
