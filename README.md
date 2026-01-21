
```md
# TaskList API (ASP.NET Core)

## Overview
TaskList API is a RESTful backend built with **ASP.NET Core 8** that supports user authentication and task management for the Task List Application.  
It uses **JWT-based authentication** and **Entity Framework Core** with SQL Server.

In addition to core task management, the API now includes **AI-powered features** for task summarization and document analysis to improve productivity and user experience.

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

### 🤖 AI Features
- **AI Task Summary**
  - Generates a **user-friendly, conversational summary** of tasks
  - Includes:
    - Overview of total number of tasks
    - Tasks due today
    - Tasks due in the next 7 days
  - Task grouping (today / next 7 days) is handled **server-side** for accuracy
  - AI interprets workload instead of listing raw task data

- **AI Document Analysis**
  - Upload documents to extract task-related information
  - Supported file types:
    - `.txt`
    - `.pdf`
    - `.docx`
  - AI extracts:
    - Short document summary
    - Action items / tasks
    - Due dates
    - People or email mentions

- **Create Tasks from AI Results**
  - Extracted tasks can be added directly to the task list
  - People mentioned are appended to task descriptions for traceability

- **Local AI (No Paid API Required)**
  - Uses **Ollama + LLaMA models**
  - Runs fully locally
  - No OpenAI or paid API keys required


## Tech Stack
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt password hashing
- Swagger (OpenAPI)
- Ollama (Local LLaMA AI)
- Angular (Frontend)

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

````

---

## Getting Started

### Prerequisites
- .NET SDK 8.0+
- SQL Server
- Visual Studio / VS Code
- Ollama (for AI features)

---

### Setup Instructions

1. **Clone the repository**
```bash
git clone https://github.com/ZaldyJegPiraman/TaskList.Api.git
cd TaskList.Api
````

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

4. **Install & run Ollama**
   Download from:

```bash
https://ollama.com/download
```

Pull the model:

```bash
ollama pull llama3:8b-instruct-q4_K_M
```

Start Ollama:

```bash
ollama serve
```

5. **Run the API**

```bash
dotnet run
```

6. **Open Swagger**

```
https://localhost:7151/swagger
```

---

## Authentication Flow

* Register user → `/api/auth/register`
* Login user → `/api/auth/login`
* JWT token returned
* Token required in `Authorization` header:

```
Authorization: Bearer <token>
```

---

## API Endpoints

### Auth

| Method | Endpoint           | Description   |
| ------ | ------------------ | ------------- |
| POST   | /api/auth/register | Register user |
| POST   | /api/auth/login    | Login user    |

### Tasks (Authorized)

| Method | Endpoint        | Description    |
| ------ | --------------- | -------------- |
| GET    | /api/tasks      | Get user tasks |
| POST   | /api/tasks      | Create task    |
| PUT    | /api/tasks/{id} | Update task    |
| DELETE | /api/tasks/{id} | Delete task    |

### AI (Authorized)

| Method | Endpoint                 | Description               |
| ------ | ------------------------ | ------------------------- |
| GET    | /api/ai/task-summary     | Generate AI task summary  |
| POST   | /api/ai/analyze-document | Analyze uploaded document |

---

## AI Design Notes

* Task grouping is handled by backend logic
* AI is used only to **interpret and summarize**
* Safeguards ensure:

  * No tasks are removed
  * No tasks are invented
  * Due dates are not modified
* Ensures predictable and consistent summaries

---

## Security

* Passwords hashed using **BCrypt**
* JWT signed with HMAC SHA256
* User-specific data isolation
* Authorization via `[Authorize]` attribute

---

## Development Notes

* Date values handled in ISO format
* DTOs used to prevent over-posting
* AI output validated before returning to the client

---

## Future Enhancements

* Refresh Tokens
* Pagination & Filtering
* Role-based access
* Docker support
* Unit & Integration Tests
* Email reminders for due tasks

---

## Author

**Zaldy Jeg M. Piraman**
Software Engineer / Full Stack Developer
GitHub: [https://github.com/ZaldyJegPiraman](https://github.com/ZaldyJegPiraman)

---

## License

This project is for technical assessment and learning purposes.

```

---

If you want, I can now:
- Shorten this for **HR / assessment reviewers**
- Add **screenshots**
- Split **API vs Frontend README**
- Add **architecture diagram**

Just say the word 👍
```
