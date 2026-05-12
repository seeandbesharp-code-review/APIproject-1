# Microservices Architecture Planning Document

## Project Overview

WebAPIShop is a modern e-commerce platform built with .NET 9 and C#, currently structured as a layered monolith. This document outlines a plan to evolve the system into a microservices architecture, improving scalability, maintainability, and independent deployment. The proposed design decomposes the current API into focused, autonomous services based on the existing controllers, models, and domain boundaries.

---

## Microservices Breakdown

### 1. **User Service**
- **Responsibility:** Manage user registration, authentication, profiles, and roles.
- **Endpoints:**
  - `POST /api/users/register` – Register a new user
  - `POST /api/users/login` – Authenticate user and issue JWT
  - `GET /api/users/{id}` – Get user profile
  - `PUT /api/users/{id}` – Update user profile
  - `DELETE /api/users/{id}` – Delete user
- **Data Ownership:** User accounts, credentials, roles, and profile data
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, JWT, NLog

---

### 2. **Product Service**
- **Responsibility:** Manage product catalog, including CRUD operations and product search.
- **Endpoints:**
  - `GET /api/products` – List/search products
  - `GET /api/products/{id}` – Get product details
  - `POST /api/products` – Create product
  - `PUT /api/products/{id}` – Update product
  - `DELETE /api/products/{id}` – Delete product
- **Data Ownership:** Product details, inventory, pricing, images
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, Redis (for caching), NLog

---

### 3. **Category Service**
- **Responsibility:** Manage product categories and category hierarchies.
- **Endpoints:**
  - `GET /api/categories` – List categories
  - `GET /api/categories/{id}` – Get category details
  - `POST /api/categories` – Create category
  - `PUT /api/categories/{id}` – Update category
  - `DELETE /api/categories/{id}` – Delete category
- **Data Ownership:** Category definitions and relationships
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL

---

### 4. **Order Service**
- **Responsibility:** Manage customer orders, order status, and order history.
- **Endpoints:**
  - `POST /api/orders` – Create new order
  - `GET /api/orders/{id}` – Get order details
  - `GET /api/orders/user/{userId}` – List orders for a user
  - `PUT /api/orders/{id}/status` – Update order status
- **Data Ownership:** Orders, order items, order status, payment references
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, NLog

---

### 5. **Rating & Review Service**
- **Responsibility:** Manage product ratings and reviews, and track API usage analytics.
- **Endpoints:**
  - `POST /api/ratings` – Submit a rating/review
  - `GET /api/ratings/product/{productId}` – Get ratings for a product
  - `GET /api/ratings/user/{userId}` – Get ratings by a user
- **Data Ownership:** Ratings, reviews, analytics logs (e.g., traffic monitoring)
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL

---

## Communication Patterns

- **API Gateway:** Expose a unified entry point for clients, routing requests to appropriate services.
- **Synchronous HTTP (REST):** Services communicate via RESTful HTTP APIs for most operations.
- **Asynchronous Messaging (optional):** Use a message broker (e.g., RabbitMQ, Azure Service Bus) for cross-service events such as "OrderPlaced" or "UserDeleted" to decouple services and enable eventual consistency.
- **Service Discovery:** Use service registry (e.g., Consul, Eureka) if dynamic service location is needed.

---

## Data Ownership & Boundaries

- **Each service owns its own database schema** and is responsible for its data integrity.
- **No direct database sharing** between services; all cross-service data access must go through service APIs.
- **Data duplication** (e.g., product name in order records) is allowed for denormalization and resilience.

---

## Technology Suggestions

- **.NET 9 Web API** for all services (consistent with current stack)
- **Entity Framework Core** for data access
- **SQL Server or PostgreSQL** as primary data stores
- **Redis** for caching in Product Service
- **NLog** for logging
- **JWT** for authentication (User Service as the authority)
- **API Gateway** (e.g., YARP, Ocelot, or Azure API Management)
- **Docker** for containerization
- **Kubernetes** for orchestration (optional, for large-scale deployments)
- **Swagger/OpenAPI** for API documentation

---

## Summary

This microservices plan decomposes WebAPIShop into focused, independently deployable services aligned with business domains. Each service owns its data and exposes a clear API. Communication is primarily RESTful, with optional asynchronous messaging for decoupled workflows. The technology stack leverages .NET 9 and familiar tools to ensure a smooth transition from the current monolith.

---
