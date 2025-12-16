# TaskList API (ASP.NET Core)

## Overview
TaskList API is a RESTful backend built with **ASP.NET Core 8** that supports user authentication and task management for the Task List Application.  
It uses **JWT-based authentication** and **Entity Framework Core** with SQL Server.

---

## Features
- User Registration
- User Login with JWT Authentication
- Secure Task CRUD operations (per-user)
- Task properties:
  - Title
  - Description
  - Due Date
  - Priority (Low, Medium, High)
  - Category
  - Status (To Do, In Progress, Completed)

---

## Tech Stack
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt password hashing
- Swagger (OpenAPI)

---

## Project Structure
```
TaskList.Api
├── Controllers
│   ├── AuthController.cs
│   └── TasksController.cs
├── Data
│   └── AppDbContext.cs
├── DTOs
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   └── CreateTaskDto.cs
├── Models
│   ├── User.cs
│   ├── TaskItem.cs
│   ├── TaskPriority.cs
│   └── TaskStatus.cs
├── Services
│   └── JwtService.cs
├── Program.cs
└── appsettings.json
```

---

## Getting Started

### Prerequisites
- .NET SDK 8.0+
- SQL Server
- Visual Studio / VS Code

---

### Setup Instructions

1. **Clone the repository**
```bash
git clone https://github.com/ZaldyJegPiraman/TaskList.Api.git
cd TaskList.Api
```

2. **Update connection string**
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskListDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

3. **Apply migrations**
```bash
dotnet ef database update
```

4. **Run the API**
```bash
dotnet run
```

5. **Open Swagger**
```
https://localhost:7151/swagger
```

---

## Authentication Flow
- Register user → `/api/auth/register`
- Login user → `/api/auth/login`
- JWT token returned
- Token required in `Authorization` header:
```
Authorization: Bearer <token>
```

---

## API Endpoints

### Auth
| Method | Endpoint | Description |
|------|---------|------------|
| POST | /api/auth/register | Register user |
| POST | /api/auth/login | Login user |

### Tasks (Authorized)
| Method | Endpoint | Description |
|------|---------|------------|
| GET | /api/tasks | Get user tasks |
| POST | /api/tasks | Create task |
| PUT | /api/tasks/{id} | Update task |
| DELETE | /api/tasks/{id} | Delete task |

---

## Security
- Passwords hashed using **BCrypt**
- JWT signed with HMAC SHA256
- User-specific data isolation
- Authorization via `[Authorize]` attribute

---

## Development Notes
- Date values handled in ISO format
- Enums mapped explicitly to prevent casting issues
- DTOs used to prevent over-posting

---

## Future Enhancements
- Forgot Password / Reset Password
- Refresh Tokens
- Pagination & Filtering
- Role-based access
- Docker support
- Unit & Integration Tests

---

## Author
**Zaldy Jeg M. Piraman**  
Software Engineer / Full Stack Developer  
GitHub: https://github.com/ZaldyJegPiraman

---

## License
This project is for technical assessment and learning purposes.
