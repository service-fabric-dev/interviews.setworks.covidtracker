# 🦠 CovidTracker

A real-time, distributed COVID-19 data monitoring application built with .NET Aspire, CQRS, Domain-Driven Design (DDD), and SignalR.

## 🏗️ Architecture Overview

CovidTracker is a modular, message-driven system that ingests, stores, and visualizes real-time COVID-19 statistics. It leverages Clean Architecture and CQRS with MassTransit, and supports push notifications via SignalR to a Blazor Server UI.

### 🧱 Projects

| Project                  | Role |
|--------------------------|------|
| `CovidTracker.Domain`    | Core domain model: aggregates, entities, value objects, domain services, and event interfaces |
| `CovidTracker.Application` | CQRS handlers, use case coordination, domain orchestration logic |
| `CovidTracker.Infrastructure` | Implements domain/service interfaces (e.g., repositories, SignalR, MassTransit, API clients) |
| `CovidTracker.ApiService` | ASP.NET Core Web API host: exposes CQRS endpoints, schedules background ingestion jobs |
| `CovidTracker.Web`       | Blazor Server UI: renders stat map and listens to real-time updates via SignalR |
| `CovidTracker.WorkerService` | Background consumer host: listens for integration events, updates state, and notifies clients |
| `CovidTracker.AppHost`   | .NET Aspire orchestration layer: wires services and infrastructure together for local dev |

---

## 🔧 Technologies

| Technology                | Purpose                                                           |
| ------------------------- | ----------------------------------------------------------------- |
| **.NET Aspire**           | Local orchestration and dashboard for services and infrastructure |
| **Blazor Server**         | Real-time, stateful web UI with SignalR                           |
| **MassTransit**           | Message bus abstraction over Azure Service Bus                    |
| **MassTransit Mediator**  | CQRS command/query dispatching within each service                |
| **Entity Framework Core** | Data access (SQL Server) via repositories                         |
| **Azure SQL Database**    | Persistent backing store for COVID stats and alerts               |
| **Azure Service Bus**     | Asynchronous messaging between services                           |
| **SignalR**               | Real-time communication with connected Blazor clients             |
| **disease.sh API**        | External COVID-19 data provider                                   |

---

## 📂 Domain-Driven Design Layers

Domain
- Aggregates
- Entities
- ValueObjects
- Interfaces (e.g., IStatRepository, IDomainPublisherService)

Application
- Commands
- Queries
- C/Q Handlers
- Orchestrators / Services

Infrastructure
- Repositories (EF Core)
- MassTransit publishers
- SignalR push services
- Covid API client

---

## 🛠️ Running the Solution

> Requires .NET 8 SDK and Docker (for Aspire resources like SQL Server, RabbitMQ, etc.)

```powershell
dotnet run --project CovidTracker.AppHost
```

* This boots up:

  * `ApiService`
  * `Web` (Blazor UI)
  * `WorkerService`
  * Aspire Dashboard

