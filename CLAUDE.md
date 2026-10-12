# CLAUDE.md

Guidance for working in this repository.

## Overview

A .NET 8 microservices demo of a banking system (customers, accounts, transactions). Everything lives under `src/`; the solution file is `src/MicroservicesArchitecture.sln`. Services run in Docker via `src/docker-compose.yml`.

| Component | Path | Stack | Host port |
|---|---|---|---|
| Account API | `src/Services/Account/` | Clean Architecture (API / Application / Domain / Infrastructure), MediatR, FluentValidation, EF Core + PostgreSQL | 8001 |
| Customer API | `src/Services/Customer/Customer.API` (+ Application / Domain / Infrastructure) | same layering, PostgreSQL | 8002 |
| Customer gRPC | `src/Services/Customer/Customer.GRPC` | gRPC server, shares Customer.Infrastructure | 5001 |
| Transaction API | `src/Services/Transaction/Transaction.API` | single project (Controllers / Services / Repositories), MongoDB, MassTransit consumer | 8003 |
| API Gateway | `src/ApiGateways/OcelotApiGateway` | Ocelot (routes, caching, rate limiting) | 8010 |
| WebStatus | `src/WebApps/WebStatus` | ASP.NET MVC + HealthChecks UI dashboard | 8007 |
| Building blocks | `src/BuildingBlocks/` | `Common.Logging` (Serilog → Elasticsearch), `EventBus.Messages` (shared event contracts) | – |
| NuGet package | `src/NugetPackage/ApiVersioningLib` | Published library for per-service API version registry | – |

Infra containers: PostgreSQL ×2 (accounts 5433, customers 5434), MongoDB (27017), RabbitMQ (5672 / UI 15672), pgAdmin, mongo-express, Portainer, Elasticsearch + Kibana.

## Commands

Run from `src/` unless noted.

```bash
# First time: cp .env.example .env  (compose reads credentials from src/.env, git-ignored)
# Whole system (builds with --no-cache, then docker compose up)
./scripts/project/start_project.sh
docker compose up -d --build            # faster, uses cache

# Build / test
dotnet build MicroservicesArchitecture.sln
dotnet test Services/Account/Account.Test/Account.Test.csproj
dotnet test Services/Account/Account.Test/Account.Test.csproj --filter "FullyQualifiedName~ArchitectureTests"
```

- Only the Account service has tests (xUnit, Moq, EF InMemory, NetArchTest). CI (`.github/workflows/account_api_test.yml`) runs them on pushes/PRs to `develop`.
- `scripts/databases/*/*.sh` are psql/mongo helper scripts for inspecting data (need the containers up on localhost).
- Swagger is available at `/` (redirects to `/swagger`) on each API in Development. Health endpoint: `/health-check` on every service.

## Architecture & data flow

- **Gateway** (`ocelot.json`, `ocelot.Development.json`): clients call the gateway on 8010 (`/accounts`, `/accounts/{id}`, `/accounts/{id}/deposit`, …); it forwards to `/api/...` on the downstream services using docker hostnames (`account_api:8080`, etc.). When adding/changing a controller route, update both Ocelot files.
- **Account → Customer (sync, gRPC)**: `AccountsController` calls `CustomerGrpcService.CheckCustomer` before creating an account / listing a customer's accounts. Proto: `Customer.GRPC/Protos/customer.proto`. URL comes from `GrpcSettings:CustomerUrl`.
- **Account → Transaction (async, RabbitMQ via MassTransit)**: after a successful deposit/withdraw the controller publishes `AccountTransactionEvent` (`BuildingBlocks/EventBus.Messages`). `Transaction.API`'s `AccountTransactionConsumer` listens on `EventBusConstants.AccountTransactionQueue`, maps it with AutoMapper and stores it in MongoDB. Changing the event contract means touching both sides.
- **Account.Application** is CQRS with MediatR: `Features/Accounts/{Commands,Queries}/<UseCase>/` each with Command/Query, Handler, Validator (FluentValidation, wired through `Behaviours/ValidationBehaviours.cs`), and Response. Domain exceptions (`EntityNotFoundException`, `InsufficientBalanceException`, `UnauthorizedAccessException`) are mapped to 404 / 400 / 403 by `GlobalExceptionHandlingMiddleware`.
- Layering is enforced by `Account.Test/ArchitectureTests.cs`: API must not reference Domain directly; Infrastructure must not reference API. Keep it that way.
- **Startup behaviour**: Account and Customer APIs call `Database.MigrateAsync()` on boot; when `RunMigrations=true` (set in docker-compose) they also seed from JSON (`Accounts.json`, `Transactions.json`, seed classes). Transaction.API seeds Mongo under the same flag.
- **API versioning**: each service reads `APIVersion` from appsettings and names its Swagger doc `v{APIVersion}`.

## Conventions

- C# 12 / .NET 8, primary constructors, file-scoped namespaces in most places (some older files use block namespaces and a 4-space-indented brace style — follow the file you edit).
- Config via `appsettings.json` with docker hostnames (`rabbitmq`, `elasticsearch`, `customer_grpc`); docker-compose overrides connection strings with `ConnectionStrings__*` env vars. Running a service outside Docker means overriding these.
- EF migrations live in each `*.Infrastructure/Migrations`. Add one with `dotnet ef migrations add <Name> -p <Infrastructure project> -s <API project>`.
- Branches: `<feat|fix|docs|refactor|test|chore>/<kebab-description>`, PR into `develop`. Commits use Conventional Commits. See `CONTRIBUTING.md`.
- Formatting is defined in `.editorconfig`; CI runs `dotnet format whitespace --verify-no-changes` (excluding Migrations). Run `dotnet format whitespace MicroservicesArchitecture.sln --exclude '**/Migrations/**'` before committing.

## Gotchas

- Compose credentials come from `src/.env` (template: `src/.env.example`). `appsettings.json` only holds dev defaults (e.g. `guest:guest` RabbitMQ) overridden by compose env vars. Never commit `.env` or real secrets.
- `GlobalExceptionHandlingMiddleware` returns stack traces in responses; fine for the demo, not for production.
- No authentication/authorization is configured (`UseAuthorization` only); "authenticated user" endpoints take `customerId` as a parameter.
- `MassTransit.AspNetCore` is pinned to 7.3.1 while `MassTransit` is 8.2.5 in some projects — watch for version mismatches when touching the event bus.
- `ApiVersioningLib` is published as a NuGet package; bump its version in the csproj and README when changing it.
- Elasticsearch dependency in Account.API's compose entry is commented out; Serilog's ES sink may log connection errors if the container isn't running.
