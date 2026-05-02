# ⚙️ Smart City – Backend (.NET Core API)

This is the backend of the **Smart City Issue Management System**, built using ASP.NET Core with Clean Architecture.

It handles authentication, issue management, real-time updates, and map-based data processing.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- JWT-based authentication
- Role-based access:
  - Admin
  - Worker
  - Citizen

---

### 📌 Issue Management
- Create issues with:
  - Location (latitude, longitude)
  - Image upload
- Assign workers to issues
- Track issue lifecycle:
  - Reported → Assigned → InProgress → Resolved → Rejected
- Upload resolution image

---

### ⚡ Real-Time System (SignalR)
- Live updates for:
  - Issue creation
  - Assignment
  - Resolution
- No polling required
- Clean architecture implementation using abstraction (no direct dependency in Application layer)

---

### 🗺️ Map API
- Fetch issues within radius
- Supports:
  - Nearby issue detection
  - Duplicate prevention
  - Admin map visualization

---

### 📦 File Upload
- Integrated with **Cloudinary**
- Stores:
  - Issue images
  - Resolution images

---

## 🏗️ Architecture

Follows **Clean Architecture principles**:

API Layer → Controllers, SignalR Hubs
Application → CQRS (Commands & Queries), DTOs
Domain → Entities, Enums, Business Rules
Infrastructure → Database, Repositories, External Services
---

## 🧠 Design Patterns

- CQRS (Command Query Responsibility Segregation)
- MediatR
- Repository Pattern
- Dependency Injection
- Interface-based abstraction for SignalR

---

## 🛠️ Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- MediatR
- SignalR
- SQL Server
- Cloudinary (image storage)

---

## 📂 Project Structure

SmartCity.API # Controllers + SignalR Hub
SmartCity.Application # Business logic (CQRS)
SmartCity.Domain # Core entities
SmartCity.Infrastructure # DB + external services


---

## ▶️ How to Run the Backend

### 📌 Prerequisites

- .NET SDK (6 or above)
- SQL Server
- Visual Studio / VS Code
- Postman / Swagger (optional)

---

### ⚙️ Step 1 — Clone Repository

git clone https://github.com/your-username/smart-city-backend.git
cd smart-city-backend 

### ⚙️ Step 2 — Configure Database

Open the `appsettings.json` file and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SmartCityDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
dotnet ef database update
