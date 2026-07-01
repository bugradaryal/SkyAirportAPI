# SkyAirport API (.NET Core)

A RESTful API built with .NET Core for managing aviation operations such as flights, airports, routes, and crew data. The project follows a layered architecture and focuses on maintainable backend design.

---

## 🏛️ Architecture & Application Design

* **N-Tier Architecture:** The system is structured into API, Business, DataAccess, and Entities layers to ensure separation of concerns.
* **CQRS Pattern:** Implemented with MediatR to separate command and query logic.
* **Validation:** FluentValidation is integrated via MediatR pipeline behavior for centralized request validation.
* **Data Access:** Entity Framework Core (Code-First) is used for database modeling and persistence.

---

## 🧱 Infrastructure (Docker)

The entire system is containerized using Docker Compose.

It includes:
- API (.NET Core Web API)
- PostgreSQL
- Redis
- Elasticsearch + Kibana

This setup allows the full stack to run in an isolated and reproducible environment with a single command.

---

## 🔍 Logging & Observability

* Serilog is used for structured logging.
* Logs are stored in PostgreSQL and sent to Elasticsearch for centralized search and analysis.
* Console logging is enabled for local debugging.

---

## ⚡ Performance & Background Processing

* Redis is used for caching frequently accessed data.
* Hangfire handles background and scheduled jobs.

---

## 🔐 Security

* JWT-based authentication with ASP.NET Core Identity.
* Role-based authorization for protected endpoints.

---

## 🛠️ Tech Stack

.NET Core, ASP.NET Core Web API, Entity Framework Core (Code-First), PostgreSQL, Redis, Elasticsearch, Kibana, Docker, MediatR, FluentValidation, Hangfire, Serilog
