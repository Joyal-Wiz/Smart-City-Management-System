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
- Decoupled using **Clean Architecture (interface-based SignalR integration)**

---

### 🗺️ Map API
- Fetch issues within radius
- Support for:
  - Nearby issue detection
  - Duplicate prevention
  - Admin map visualization
- Optimized queries for performance

---

### 📦 File Upload
- Integrated with **Cloudinary**
- Stores:
  - Issue images
  - Resolution images

---

## 🏗️ Architecture

Follows **Clean Architecture principles**:

```text
API Layer        → Controllers, SignalR Hubs
Application      → CQRS (Commands & Queries), DTOs
Domain           → Entities, Enums, Business Rules
Infrastructure   → Database, Repositories, External Services
