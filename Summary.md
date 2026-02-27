# Task.API - Project Summary

## Project Overview

Task.API is a modern, secure RESTful Web API built with ASP.NET Core 8.0 that provides comprehensive task management functionality. The project implements JWT-based authentication, Entity Framework Core with SQLite database, and follows clean architecture principles.

## Key Features

### 1. User Authentication
- JWT (JSON Web Token) based authentication
- User registration and login endpoints
- Secure password hashing via ASP.NET Core Identity
- Token-based stateless authentication

### 2. Task Management
- **Create Tasks**: Add new tasks with title, category, and assignee
- **Read Tasks**: Retrieve all tasks or individual tasks by ID
- **Update Tasks**: Modify task status (Pending, InProgress, Completed, etc.)
- **Delete Tasks**: Remove tasks from the system

### 3. Task Properties
- Title: Task name/description
- Category: Task categorization
- CategoryColor: Visual color coding for categories
- AssignedUser: User assigned to the task
- Status: Current task status
- CreatedAt/UpdatedAt: Timestamps

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 8.0 |
| Language | C# 12 |
| Database | SQLite |
| ORM | Entity Framework Core |
| Authentication | JWT Bearer Tokens |
| API Documentation | OpenAPI / Scalar |
| Identity | ASP.NET Core Identity |

## Project Components

### Controllers
- **TasksController**: REST API endpoints for task CRUD operations
- **Identity API**: Built-in ASP.NET Core Identity endpoints for auth

### Data Layer
- **ApplicationDbContext**: Entity Framework Core context
- **ApplicationDbContextFactory**: DbContext factory for migrations

### Models
- **TaskItem**: Domain entity for tasks
- **ApplicationUser**: Custom user entity extending IdentityUser

### DTOs
- **TaskDtos**: Data transfer objects for API requests/responses

### Enums
- **TaskStatus**: Task status enumeration (Pending, InProgress, Completed, etc.)

## API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/register | Register new user |
| POST | /api/auth/login | Login and get JWT token |

### Tasks
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/tasks | Get all tasks |
| GET | /api/tasks/{id} | Get task by ID |
| POST | /api/tasks | Create new task |
| PUT | /api/tasks/{id}/status | Update task status |
| DELETE | /api/tasks/{id} | Delete task |

## Configuration

### Database
- SQLite database stored at `Data/tasks.db`
- Connection string configured in `appsettings.json`

### JWT Settings
- Key: Configurable in appsettings (default provided for development)
- Issuer: Task.API
- Audience: Task.APIUsers

### Environment
- Development: Includes OpenAPI/Scalar documentation
- Production: Optimized for deployment

## Build & Run

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Commands
```
bash
# Restore dependencies
dotnet restore

# Run database migrations
dotnet ef database update

# Run the application
dotnet run
```

## Security Considerations

1. **HTTPS**: Forced HTTPS redirection in production
2. **Authentication**: JWT tokens with configurable expiration
3. **Password Storage**: BCrypt hashing via ASP.NET Core Identity
4. **Input Validation**: Model validation on all DTOs

## Future Enhancements

- Add unit and integration tests
- Implement pagination for task listing
- Add task filtering and sorting
- Implement role-based authorization
- Add file attachment support
- Implement task comments/notes
- Add notification system
- Implement webhooks for external integrations

## License

This project is available for personal and commercial use under the MIT License.
