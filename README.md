# 🛒 WebAPIShop
### **Modern RESTful API | .NET 9 | C# | Layered Architecture**

---

## 📖 Overview
**WebAPIShop** is a professional **REST Web API** built with **.NET 9** and **C#**. The project strictly adheres to **RESTful principles**, providing a standardized and scalable way to interact with data over HTTPS. It is designed with a focus on high performance, maintainability, and **Clean Code**.

---

## 🏗️ Architecture & Design Patterns

The project is structured using a **Layered Architecture** to achieve total **Separation of Concerns**:



📱 **Application Layer** – Handles API controllers, routing, and ensures **REST principles** are followed.

⚙️ **Service Layer** – Contains all **Business Logic**, facilitating communication between layers.

🗄️ **Repositories Layer** – Manages **Data Access** logic and database communication.

### Key Technical Features:
💉 **Dependency Injection (DI):** Implemented across all layers to create **Decoupling** and improve system flexibility.

⚡ **Asynchronous Programming:** Database access is handled **Asynchronously** to free up threads and ensure maximum **Scalability**.

🗃️ **Entity Framework Core (ORM):** Developed using the **DB-First** approach for efficient data management.

📦 **DTOs & Records:** Uses **C# Records** for **Data Transfer Objects** to remove circular dependencies and decouple the Data layer from the API layers.

🔄 **AutoMapper:** Used for automatic and clean mapping between Database  and DTOs.

⚙️ **Configuration:** Settings are managed via appsettings.json to keep the code clean and environment-flexible.

---

## 📁 Project Structure



```text
├── WebAPIShop/           # Entry point, controllers, middleware
├── Services/             # Business logic implementations
├── Repositories/         # Data access implementations
├── /             # Domain models (Database )
├── DTOs/                 # Record-based data transfer objects
├── Tests/                # xUnit test projects (Unit & Integration)
└── appsettings.json      # External configuration
```


## 🛡️ Reliability & Monitoring

A robust system requires proactive monitoring and error management to ensure stability and high availability:

| Feature | Description |
| :--- | :--- |
| **Global Error Handling** | A custom **Middleware** that intercepts all exceptions globally, providing consistent API responses and preventing system crashes. |
| **NLog Integration** | Extensive implementation of **NLog** for detailed recording of system events, warnings, and error diagnostics. |
| **Traffic Monitoring** | All incoming server requests are tracked and logged into a dedicated **Rating table** for auditing, analytics, and performance monitoring. |

---

## 🧪 Testing Suite

We maintain high reliability using the **xUnit** library with a comprehensive testing strategy:

✅ **Unit Tests:** Validating individual business logic units in isolation to ensure correctness.
✅ **Integration Tests:** Ensuring the entire data flow between layers and the database works seamlessly together.

---

## 🛠️ Tech Stack

**Framework:** .NET 9 🚀
**Language:** C#
**ORM:** Entity Framework Core
**Mapping:** AutoMapper
**Logging:** NLog
**Testing:** xUnit

---

## 🚀 Getting Started

### Prerequisites
**.NET 9 SDK**
A supported database (SQL Server / PostgreSQL / etc.)

### Run the API
```bash
# Restore dependencies
dotnet restore

# Apply migrations / Update database
dotnet ef database update

# Run the project
dotnet run --project WebAPIShop
```

### 🧪 Run Tests

To ensure the stability and reliability of the system, you can run the full test suite (Unit and Integration tests) using the following command:

```bash
# Execute all xUnit tests
dotnet test
```

## 📄 License

This project is licensed under the **MIT License**.

---
**Ayala & Sarli**  
<small>2026</small>