# SkyAirport API

A layered, production-style RESTful API built with **.NET Core** for managing airport and airline operations — flights, aircraft, seats, tickets, and personnel. The project focuses on clean architecture, maintainability, and real-world backend concerns like caching, background processing, and observability rather than being a simple CRUD demo.

---

## 🏛️ Architecture

- **N-Tier Architecture** — the solution is split into `API`, `Business`, `DataAccess`, `Entities`, `DTO`, and `Utilitys` projects, keeping concerns cleanly separated.
- **CQRS with MediatR** — commands and queries are implemented as isolated request/handler pairs, organized by feature (e.g. `Account/Commands/AddRoleToUser`, `Account/Queries/Login`).
- **Generic Repository & Generic CQRS Handlers** — since MediatR doesn't provide generic CRUD out of the box, a fully generic CQRS layer was built on top of it: open generic requests (`GenericAddRequest<T>`, `GenericGetAllRequest<T>`, `GenericUpdateRequest<T>`, `GenericDeleteRequest<T>`, `GenericGetByIdRequest<T>`) are paired with open generic handlers (`GenericAddHandle<T>`, `GenericGetAllHandler<T>`, etc.), each resolving a `GenericRepository<T>` to perform the actual persistence. This removes repetitive CRUD boilerplate across entities such as `Seat`, `Ticket`, `Flight`, `Aircraft`, `Airline`, `Airport`, `Crew`, and `Personal`, while entity-specific queries (e.g. `GetAllFlightByAirlineId`) are still implemented as regular, non-generic MediatR handlers where custom logic is needed.
- **`IConvertToEntity<T>`** — add/update requests implement this interface, giving generic handlers a consistent way to obtain the entity to persist regardless of how the request was constructed.
- **Custom AutoMapper Wrapper** — a lightweight `Mapper` service dynamically registers and caches type pairs at runtime instead of requiring a static mapping profile per DTO.

## 🔐 Security

- **JWT Bearer Authentication** with refresh token support (`TokenManager`), including token validation directly from the request context.
- **Custom API Key Middleware** — a secondary `X-API-KEY` header check is enforced on all `/api` routes, layered on top of JWT auth.
- **ASP.NET Core Identity** — role-based authorization (`Administrator`, `Support`, `User`), seeded on startup.
- **Account suspension** — suspended users carry an `IsSuspended` claim enforced via a custom authorization policy.
- **Rate Limiting** — a fixed-window limiter partitions requests per user/IP, with admins exempt from limits.

## 🔍 Logging & Observability

- **Fully automatic request logging** — `LoggingMiddleware` logs every request without any manual calls in the handlers. If the hit endpoint is decorated with `[LogAction(Action_Type.X)]`, the request is logged once (after execution) with that action type; otherwise it falls back to logging an `APIRequest` on the way in and an `APIResponse` on the way out.
- **`LogActionAttribute`** — a `[LogAction(...)]` attribute (method or class level) is the only thing a controller/action needs to opt into a specific `Action_Type` (`Create`, `Update`, `Delete`, etc.) instead of the generic request/response pair.
- **`LogLevelResolver`** — maps an HTTP status code to a log severity (`>=500` → Error, `>=400` → Warn, else Info) shared by both `LoggingMiddleware` and `ExceptionMiddleware`, so severity is derived consistently in one place instead of being decided ad hoc per call site.
- **`ExceptionMiddleware`** — catches unhandled exceptions globally, resolves the status code from `CustomException.ErrorCode` (or 500), logs it through the same `ILoggerServices` pipeline, and returns a JSON error response — so an exception never needs an explicit logging call either.
- **`LoggerManager`** (`ILoggerServices`) is the single sink all logging funnels through: it maps the incoming `LogDTO` to a `LogEntry`, writes to **Serilog** (`ISerilogServices`/`SerilogLogger`) at the resolved level, and persists the same entry to the database via `ILogRepository`. If logging itself throws, it falls back to a `Fatal`-level "Critical Fatal Error" record so a logging failure is never silent.
- Every log entry is tagged with an `Action_Type`, a `Target_table` (the request path), an optional `user_id` (from the `uid` claim), and `AdditionalData`, making individual log records traceable back to the endpoint, entity, and user involved.
- **Elasticsearch + Kibana** — logs are shipped for centralized search and visualization.

## ⚡ Caching & Background Processing

- **Redis** caches frequently accessed data, including a live currency exchange rate used to convert ticket prices on demand.
- **Hangfire** (backed by PostgreSQL storage) runs scheduled and background jobs — including a recurring job that fetches USD/EUR exchange rates from the Turkish Central Bank (TCMB) XML feed and caches them in Redis.

## ✅ Validation

- **FluentValidation** is wired into the MediatR pipeline, so requests like login are validated before reaching business logic.

## ✉️ Notifications

- **Email verification** via MailKit/SMTP, with a confirmation-token flow through ASP.NET Identity.
- Phone/SMS notification service is scaffolded for future implementation.

## 🧱 Infrastructure

The system is designed to run via Docker Compose, including:

- API (.NET Core Web API)
- PostgreSQL
- Redis
- Elasticsearch + Kibana

## 🛠️ Tech Stack

.NET Core · ASP.NET Core Web API · Entity Framework Core (Code-First) · PostgreSQL · Redis · Elasticsearch · Kibana · Docker · MediatR (CQRS) · FluentValidation · AutoMapper · Hangfire · Serilog · JWT · ASP.NET Core Identity · MailKit

## 📄 API Documentation

Swagger/OpenAPI is enabled in development, with both JWT Bearer and API Key security schemes configured for testing protected endpoints directly from the Swagger UI.

## 🚀 Getting Started

```bash
docker-compose up -d
```

This spins up the supporting infrastructure (PostgreSQL, Redis, Elasticsearch, Kibana). Run the API project separately (`dotnet run` or via your IDE) once the containers are healthy.

## 🗄️ Database Configuration

The project ships with three `appsettings` files to cover three different run scenarios. Pick the one that matches how you're running the API:

| File | Used when | Port | Host |
|---|---|---|---|
| `appsettings.json` | Running the API natively (default, no extra setup) | 5432 | localhost |
| `appsettings.Docker.json` | API running **inside a container** (`ASPNETCORE_ENVIRONMENT=Docker`), loaded automatically by `docker-compose up` | 5432 (internal) | `postgres` (container name) |
| `appsettings.DockerHost.json` | Reaching the Dockerized Postgres **from the host** — for running migrations, or hybrid debugging | 5433 | localhost |

> Port 5433 is used for host access because native Postgres already listens on 5432; the container maps `5433:5432` in `docker-compose.yml` to avoid a conflict.

**Scenario 1 — Native / local:** just run the API (`dotnet run` or F5 with the default profile). Uses `appsettings.json` against native Postgres (5432).

**Scenario 2 — Fully Dockerized (API + DB in containers):**
```bash
docker-compose up -d --build
cd API
dotnet ef database update --project DataAccess --startup-project API -- --environment DockerHost
```
`docker-compose up` starts the API with `appsettings.Docker.json` (internal 5432). Migrations are applied from the host via the `DockerHost` environment (5433) against the same physical database, so the containerized API picks them up. Re-run the `dotnet ef` command whenever a new migration is added.

**Scenario 3 — Hybrid (API on host, DB in Docker):**
```bash
docker-compose up -d postgres
dotnet run --launch-profile DockerDB-Local
```
Uses `appsettings.DockerHost.json` (5433). Mainly useful for debugging with breakpoints on the host while the DB runs in Docker.

**Notes:**
- `dotnet ef` does not read `launchSettings.json` profiles — always pass `-- --environment <name>` explicitly when running migrations.
- On a new machine: install Docker Desktop, clone the repo, install the EF tool (`dotnet tool install --global dotnet-ef`), then follow Scenario 2.
- `appsettings.DockerHost.json` contains a password and would normally be excluded from source control; it currently holds a local dev-only password, but should move to User Secrets / environment variables before targeting a real environment.

---

## 📌 Domain Overview

The API models a broad slice of real-world airport/airline operations:

- **Airports** — top-level entities owning a collection of `Airline`s and `Personal` (staff).
- **Airlines** — belong to an `Airport`, own a collection of `Flight`s; queryable standalone or by airport (`GetAllAirlinesByAirportId`).
- **Aircraft & Aircraft Status** — aircraft data is split across two entities: `Aircraft` holds technical specs (`Model`, `Fuel_Capacity`, `Max_Altitude`, `Engine_Power`, `Last_Maintenance_Date`) plus `Carry_Capacity`/`Current_Capacity` for baggage tracking, while a separate `AircraftStatus` entity tracks the operational state (`Available`, `InMaintenance`, `OutOfService`, `NotOperational`) via a one-to-many relationship. Splitting the two lets operational status be queried/updated independently of the aircraft's physical properties.
- **Flights** — belong to an `Airline`, linked to `Aircraft` through a `Flight_Aircraft` many-to-many join entity, own a collection of `Seat`s and an optional `OperationalDelay`; queryable by airline or by aircraft.
- **Crew** — linked to `Aircraft` through a `Crew_Aircraft` many-to-many join entity, restricted to administrators.
- **Operational Delays** — one-to-one with a `Flight`, recording delay reason, duration, and timestamp; queryable per flight.
- **Seats** — belong to a `Flight`, one-to-one with a `Ticket`, carry class/location/availability.
- **Tickets** — one-to-one with a `Seat`, hold the price; support dynamic currency conversion (TRY, USD, EUR) via live forex rates cached in Redis.
- **Owned Tickets** — the purchase record linking a `User` to a `Ticket`, storing purchase date and baggage weight, with capacity-aware business rules (see below).
- **Personnel** — belong to an `Airport`, restricted to administrators.
- **Accounts** (`User`, extends ASP.NET Identity) — full lifecycle management: registration (with default role assignment), login, password changes, role assignment/removal (with the base `User` role protected from removal), and account suspension via an `IsSuspended` flag — all authorization- and role-aware.
- **Logging** — `LogEntry` records are linked to a `User` and a `LogLevel` (`Trace` → `Emergency`), carrying an `Action_Type`, target table, and structured `AdditionalData`.

Across nearly every entity, the same generic CQRS pattern is reused: **Commands** (`Add`/`Update`/`Delete`) and **Queries** (`GetAll`/`GetById`) live in physically separate namespaces (`Business.Features.Generic.Commands.*` vs `Business.Features.Generic.Queries.*`), with entity-specific queries (e.g. `GetAllFlightByAirlineIdRequest`) added on top where needed.

## 🎫 Business Logic Highlights

Beyond generic CRUD, several handlers implement real domain rules:

- **Ticket purchase flow** — before a ticket is bought, the seat's availability is checked; the associated aircraft's baggage capacity is validated against the ticket's baggage weight, rejecting the purchase if it would exceed the aircraft's carry capacity, and the seat is atomically marked unavailable.
- **Ticket cancellation** — reverses the capacity calculation on the aircraft and frees the seat back up.
- **Ticket update** — recalculates the aircraft's current capacity based on the delta between the old and new baggage weight, again enforcing the capacity limit.

This keeps aircraft capacity consistent across purchase, update, and cancellation flows rather than treating tickets as isolated CRUD records.
