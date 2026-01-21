# TaskList API (ASP.NET Core)

## Overview
TaskList API is a RESTful backend built with **ASP.NET Core 8** that supports user authentication and task management for the Task List Application.
It uses **JWT-based authentication** and **Entity Framework Core** with SQL Server.

In addition to core task management, the API now includes **AI-powered features** for task summarization and document analysis to help users better understand, prioritize, and create tasks.

---

## Features

### Core Features
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

### 🤖 AI Features

#### AI Task Summary
- Generates a **user-friendly, conversational summary** of tasks
- Summary includes:
  - Overview of total number of tasks
  - Tasks due today
  - Tasks due in the next 7 days
- Tasks are grouped and explained in natural language (not raw data)
- Uses backend-enforced task grouping to ensure **no task is omitted or reordered**

#### AI Document Analysis
- Upload and analyze documents to extract actionable task information
- Supported file types:
  - `.txt`
  - `.pdf`
  - `.docx`
- Extracts:
  - Short document summary
  - Potential tasks
  - Due dates (normalized to ISO format)
  - People or email addresses mentioned
- Extracted tasks can be reviewed and **added directly** to the task list

---

## Tech Stack
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt password hashing
- Swagger (OpenAPI)
- Local LLM integration (LLaMA / Ollama-compatible)

---

## Project Structure
```
TaskList.Api
├── Controllers
│   ├── AuthController.cs
│   ├── TasksController.cs
│   └── AiController.cs
├── Data
│   └── AppDbContext.cs
├── DTOs
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── CreateTaskDto.cs
│   └── DocumentAiResult.cs
├── Models
│   ├── User.cs
│   ├── TaskItem.cs
│   ├── TaskPriority.cs
│   └── TaskStatus.cs
├── Services
│   ├── JwtService.cs
│   ├── AiService.cs
│   └── DocumentTextExtractor.cs
├── Program.cs
└── appsettings.json
```

---

## Getting Started

### Prerequisites
- .NET SDK 8.0+
- SQL Server
- Visual Studio / VS Code
- Ollama or compatible local LLM service

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

### AI
| Method | Endpoint | Description |
|------|---------|------------|
| GET | /api/ai/summary | Generate AI task summary |
| POST | /api/ai/analyze-document | Analyze document and extract tasks |

---

## Security
- Passwords hashed using **BCrypt**
- JWT signed with HMAC SHA256
- User-specific data isolation
- Authorization via `[Authorize]` attribute

---

## Development Notes
- Date values handled in ISO format
- AI prompts are backend-controlled to prevent task loss or hallucination
- AI summaries are conversational but task grouping is enforced server-side

---

## Future Enhancements
- Refresh Tokens
- Role-based access
- Task recommendations
- AI priority suggestions
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
