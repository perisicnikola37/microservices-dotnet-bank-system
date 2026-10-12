<h1 align="center">Microservices .NET Bank System</h1>

<p align="center">
  A .NET 8 microservices reference project: customers, accounts and transactions, wired together with
  gRPC, RabbitMQ, an Ocelot API gateway and a full observability stack.
</p>

<p align="center">
  <a href="https://github.com/perisicnikola37/microservices-dotnet-bank-system/actions/workflows/ci.yml"><img alt="CI" src="https://github.com/perisicnikola37/microservices-dotnet-bank-system/actions/workflows/ci.yml/badge.svg?branch=develop" /></a>
  <a href="LICENSE"><img alt="License: MIT" src="https://img.shields.io/github/license/perisicnikola37/microservices-dotnet-bank-system" /></a>
  <img alt="Last commit" src="https://img.shields.io/github/last-commit/perisicnikola37/microservices-dotnet-bank-system/develop" />
  <img alt=".NET 8" src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white" />
  <img alt="PRs welcome" src="https://img.shields.io/badge/PRs-welcome-brightgreen" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white" />
  <img src="https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white" />
  <img src="https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white" />
  <img src="https://img.shields.io/badge/rabbitmq-%23FF6600.svg?&style=for-the-badge&logo=rabbitmq&logoColor=white" />
  <img src="https://img.shields.io/badge/MongoDB-4EA94B?style=for-the-badge&logo=mongodb&logoColor=white" />
  <img src="https://img.shields.io/badge/Kibana-005571?style=for-the-badge&logo=Kibana&logoColor=white" />
  <img src="https://img.shields.io/badge/Elastic_Search-005571?style=for-the-badge&logo=elasticsearch&logoColor=white" />
</p>

## Table of contents

- [Architecture](#architecture)
- [Services](#services)
- [Quick start](#quick-start)
- [API gateway routes](#api-gateway-routes)
- [Testing](#testing)
- [Project structure](#project-structure)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Architecture

```mermaid
flowchart LR
    Client([Client]) --> GW[Ocelot API Gateway]

    GW --> ACC[Account API]
    GW --> CUS[Customer API]
    GW --> TRX[Transaction API]

    ACC -- gRPC: CheckCustomer --> GRPC[Customer gRPC]
    ACC -- AccountTransactionEvent --> MQ{{RabbitMQ}}
    MQ --> TRX

    ACC --- PG1[(PostgreSQL<br/>accounts)]
    CUS --- PG2[(PostgreSQL<br/>customers)]
    GRPC --- PG2
    TRX --- MG[(MongoDB<br/>transactions)]

    STATUS[WebStatus<br/>Health Checks UI] -. /health-check .-> ACC
    STATUS -.-> CUS
    STATUS -.-> TRX
```

- **Synchronous** calls between services use **gRPC** (Account verifies that a customer exists).
- **Asynchronous** communication uses **RabbitMQ** through MassTransit: every deposit or withdrawal publishes an
  event that the Transaction service stores.
- Account and Customer follow **Clean Architecture** with **CQRS** (MediatR) and FluentValidation. Layering rules
  are enforced by architecture tests.

## Services

| Service | Description | Stack | Port |
|---|---|---|---|
| **Account API** | Create accounts, deposit, withdraw | ASP.NET Core, MediatR, EF Core, PostgreSQL | `8001` |
| **Customer API** | Create and read customers | ASP.NET Core, MediatR, EF Core, PostgreSQL | `8002` |
| **Customer gRPC** | Customer existence checks for other services | gRPC | `5001` |
| **Transaction API** | Transaction history (event consumer) | ASP.NET Core, MassTransit, MongoDB | `8003` |
| **Ocelot API Gateway** | Single entry point with caching and rate limiting | Ocelot | `8010` |
| **WebStatus** | Health Checks UI dashboard | ASP.NET Core MVC | `8007` |

Supporting tools: RabbitMQ management (`15672`), pgAdmin (`5050`), mongo-express (`8081`), Portainer (`9000`),
Kibana (`5601`), Elasticsearch (`9200`).

Shared code lives in `src/BuildingBlocks` (`Common.Logging`, `EventBus.Messages`) and
[`ApiVersioningLib`](src/NugetPackage/ApiVersioningLib/README.md) is published as a NuGet package.

## Quick start

**Prerequisites:** [Docker](https://docs.docker.com/get-docker/) with Compose v2. The [.NET 8 SDK](https://dotnet.microsoft.com/download)
is only needed to build or test outside Docker.

```bash
git clone https://github.com/perisicnikola37/microservices-dotnet-bank-system.git
cd microservices-dotnet-bank-system/src

cp .env.example .env          # local credentials, change them if you like
docker compose up -d --build
```

Then open:

| What | URL |
|---|---|
| Gateway | http://localhost:8010 |
| Account Swagger | http://localhost:8001 |
| Customer Swagger | http://localhost:8002 |
| Transaction Swagger | http://localhost:8003/swagger |
| Health dashboard | http://localhost:8007/healthchecks-ui |
| RabbitMQ UI | http://localhost:15672 |

Databases are seeded with sample data on first start. More details are in the
[installation guide](https://github.com/perisicnikola37/microservices-dotnet-bank-system/wiki/Installation-guide).

![Health checks dashboard](https://github.com/user-attachments/assets/cc2f7eea-649f-4fb2-809d-57dff2f013b6)

## API gateway routes

| Method | Gateway path | Downstream |
|---|---|---|
| `POST` | `/accounts` | Account API |
| `GET` | `/accounts/{id}` | Account API |
| `PUT` | `/accounts/{id}/deposit` | Account API |
| `POST` | `/accounts/{id}/withdraw` | Account API |
| `POST` | `/customers` | Customer API |
| `GET` | `/customers/{id}` | Customer API |
| `GET` | `/transactions` | Transaction API |
| `GET` | `/transactions/{accountId}` | Transaction API |

## Testing

```bash
cd src
dotnet test Services/Account/Account.Test/Account.Test.csproj
```

The Account service has unit, integration (EF Core in-memory) and architecture tests (NetArchTest).
CI builds the whole solution, verifies formatting and runs the tests on every pull request.

## Project structure

```text
src/
├── ApiGateways/OcelotApiGateway      # Ocelot gateway
├── BuildingBlocks/
│   ├── Common.Logging                # Serilog + Elasticsearch setup
│   └── EventBus.Messages             # Shared integration events
├── NugetPackage/ApiVersioningLib     # Published NuGet library
├── Services/
│   ├── Account/                      # API / Application / Domain / Infrastructure / Test
│   ├── Customer/                     # API / Application / Domain / Infrastructure / GRPC
│   └── Transaction/Transaction.API   # Event consumer + MongoDB
├── WebApps/WebStatus                 # Health Checks UI
├── scripts/                          # Helper scripts
└── docker-compose.yml
```

Transaction is intentionally a single project: it is a thin read model fed by events, so the extra layers
would add ceremony without value.

## Configuration

Credentials are not stored in `docker-compose.yml`. Copy [`src/.env.example`](src/.env.example) to `src/.env`
and edit it; the file is git-ignored. Service defaults live in each `appsettings.json` and are overridden by
environment variables in Compose (`ConnectionStrings__*`, `EventBusSettings__HostAddress`).

## Contributing

Contributions are welcome. Please read [CONTRIBUTING.md](CONTRIBUTING.md) and the
[Code of Conduct](CODE_OF_CONDUCT.md) first. Security issues: see [SECURITY.md](SECURITY.md).
See [CHANGELOG.md](CHANGELOG.md) for notable changes.

## License

Distributed under the MIT License. See [LICENSE](LICENSE).
