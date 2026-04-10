Smart City Issue Management System

A scalable backend system for managing city issues like garbage, road damage, and public complaints — built with Clean Architecture, CQRS, and real-time notifications.

🚀 Features
🔐 Authentication & Authorization
JWT-based authentication
Refresh token support
Role-based access control:
👨‍💼 Admin
👷 Worker
👤 Citizen

🧾 Issue Management
Report issues with image upload 📸
Track issue status:
Reported → Assigned → In Progress → Resolved / Rejected
Assign & reassign workers
Deadline & salary management
Resolution proof (image upload)

👷 Worker Management
Worker registration & approval system
Admin approval/rejection flow
Availability tracking

🔔 Notification System
Database-stored notifications
Role-based notifications (Admin, Worker, Citizen)
Mark as read / unread tracking
Unread count support

⚡ Real-Time Notifications (SignalR)
Instant notification delivery (no refresh required)
Group-based messaging:
Admin group
User-specific groups
Triggered on:
Issue assignment
Issue updates
Worker actions

📊 Pagination & Filtering
Paginated APIs for:
Issues
Workers
Optimized queries using EF Core

🧠 Architecture
Clean Architecture (Layered)
CQRS using MediatR
FluentValidation for request validation
Repository Pattern

🛠️ Background Processing
Deadline monitoring service
Automatic notifications for missed deadlines

🏗️ Tech Stack
ASP.NET Core (.NET 8)
Entity Framework Core
MediatR
JWT Authentication
SignalR
FluentValidation
Serilog

📂 Project Structure
SmartCitySolution
│
├── SmartCity.Domain
│   ├── Entities
│   ├── Enums
│   ├── ValueObjects
│
├── SmartCity.Application
│   ├── Features
│   ├── DTOs
│   ├── Interfaces
│   ├── Behaviors
│   └── Exceptions
│
├── SmartCity.Infrastructure
│   ├── Persistence
│   ├── Repositories
│   └── Services
│
├── SmartCity.API
│   ├── Controllers
│   ├── Hubs
│   ├── Services
│   ├── Middleware
│   └── Program.cs

🔄 Workflow Example
Issue Assignment Flow
Admin assigns issue
System:
Updates issue status
Creates assignment
Stores notification (DB)
SignalR:
Sends real-time notification to worker
Updates admin dashboard

📦 API Response Format
{
  "message": "Success message",
  "data": {},
  "errors": null
}

🧪 Current Status
Authentication ✅
Issue Management ✅
Worker Management ✅
Notification System ✅
Real-time (SignalR) ✅
Frontend (Angular) 🚧 In Progress

🔮 Upcoming Features
Angular frontend dashboard
Live notification UI
Admin analytics dashboard
Map-based issue tracking

🧑‍💻 Author
Joyal Jose
Junior .NET Full Stack Developer
