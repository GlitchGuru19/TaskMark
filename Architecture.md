# Task.API Architecture

## Overview

Task.API is a RESTful Web API built with ASP.NET Core 8.0 that provides task management functionality with JWT-based authentication. The architecture follows the standard ASP.NET Core project structure with clear separation of concerns.

## Project Structure

```
Task.API/
├── Controllers/          # API endpoints
│   └── TasksController.cs
├── Data/                 # Data access layer
│   ├── ApplicationDbContext.cs
│   └── ApplicationDbContextFactory.cs
├── Dtos/                 # Data Transfer Objects
│   └── TaskDtos.cs
├── Enums/                # Enumerations
│   └── TaskStatus.cs
├── Models/               # Domain models
│   ├── ApplicationUser.cs
│   └── TaskItem.cs
├── Properties/           # Launch settings
│   └── launchSettings.json
├── Migrations/           # EF Core migrations
├── appsettings.json      # Application configuration
├── Program.cs            # Application entry point
└── Task.API.csproj       # Project file
```

## Architecture Layers

### 1. Presentation Layer (Controllers)

The Controllers folder contains API endpoint handlers that process incoming HTTP requests and return responses.

- **TasksController**: Handles all task-related operations
  - GET /api/tasks - Retrieve all tasks
  - GET /api/tasks/{id} - Retrieve single task
  - POST /api/tasks - Create new task
  - PUT /api/tasks/{id}/status - Update task status
  - DELETE /api/tasks/{id} - Delete task

### 2. Data Layer

The Data layer manages database operations through Entity Framework Core.

- **ApplicationDbContext**: EF Core DbContext that represents a session with the database
  - Manages database connections
  - Tracks entity changes
  - Handles migrations
- **ApplicationDbContextFactory**: Factory for creating DbContext instances (useful for design-time operations)

### 3. Domain Layer (Models)

Domain models represent the core business entities.

- **TaskItem**: Represents a task with properties like Title, Category, Status, etc.
- **ApplicationUser**: Represents an authenticated user (extends IdentityUser)

### 4. DTO Layer

Data Transfer Objects (DTOs) are used to transfer data between layers.

- **TaskDtos**: Contains request/response DTOs for task operations
  - UpdateTaskStatusDto: For status update requests

### 5. Enumeration Layer

- **TaskStatus**: Defines task statuses (Pending, InProgress, Completed, etc.)

## Authentication & Authorization Flow

1. **User Registration/Login**: Users authenticate via `/api/auth/register` or `/api/auth/login`
2. **JWT Token Generation**: Server generates a JWT token containing user claims
3. **Token Validation**: Subsequent requests include the JWT token in the Authorization header
4. **Authorization**: Protected endpoints validate the token before processing requests

### JWT Configuration

- **Key**: Symmetric security key for token signing
- **Issuer**: Token issuer (configurable in appsettings)
- **Audience**: Intended token recipient (configurable in appsettings)
- **Expiration**: Token lifetime validation

## Database Design

### SQLite Database

The application uses SQLite as the database engine, stored in `Data/tasks.db`.

### Entity Relationships

- **ApplicationUser** → **TaskItem**: One-to-Many relationship
  - A user can have multiple tasks assigned to them
  - Tasks track the AssignedUser property

### Migrations

Entity Framework Core migrations are used for database schema management:
- Initial migration creates the database schema
- Subsequent migrations handle schema changes

## Security Features

1. **JWT Authentication**: Stateless token-based authentication
2. **Password Hashing**: ASP.NET Core Identity handles secure password storage
3. **HTTPS Enforcement**: Application redirects HTTP to HTTPS
4. **Authorization Policies**: Role-based access control for protected endpoints

## Configuration

Configuration is managed through `appsettings.json` and environment-specific files:

- **appsettings.json**: Default configuration
- **appsettings.Development.json**: Development-specific settings

### Key Configuration Sections

- **ConnectionStrings**: Database connection string
- **Jwt**: JWT authentication settings (Key, Issuer, Audience)
- **Logging**: Application logging levels

## Pipeline Configuration

The middleware pipeline is configured in Program.cs:

1. **OpenAPI/Scalar**: API documentation
2. **Identity API**: User authentication endpoints
3. **HTTPS Redirection**: Force HTTPS
4. **Authentication**: JWT token validation
5. **Authorization**: Policy enforcement
6. **Controllers**: Request handling

## Extension Points

The architecture can be extended in the following ways:

1. **Additional Controllers**: Add new controllers for new features
2. **Services Layer**: Introduce a service layer for business logic
3. **Validation**: Add FluentValidation for complex validation rules
4. **Caching**: Add response caching for performance
5. **Rate Limiting**: Add rate limiting for API protection
6. **Health Checks**: Add health check endpoints for monitoring

## Dependencies

- **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT authentication
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: User management
- **Microsoft.EntityFrameworkCore.Sqlite**: SQLite database provider
- **Microsoft.EntityFrameworkCore.Design**: EF Core tools
- **Scalar.AspNetCore**: OpenAPI documentation UI
