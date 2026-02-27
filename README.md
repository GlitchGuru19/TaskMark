# Task.API

A modern, secure ASP.NET Core Web API for task management built with .NET 8, Entity Framework Core, and SQLite. Features JWT authentication, user management, and comprehensive task CRUD operations.

## Features

- **User Authentication & Authorization**: JWT-based authentication with ASP.NET Core Identity
- **Task Management**: Full CRUD operations for tasks with status tracking
- **Database**: SQLite with Entity Framework Core migrations
- **API Documentation**: OpenAPI/Swagger integration with Scalar UI
- **Security**: Secure endpoints with role-based access control
- **Modern Architecture**: Clean architecture with separation of concerns

## Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQLite with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Documentation**: OpenAPI/Swagger with Scalar
- **ORM**: Entity Framework Core
- **Language**: C# 12

## Quick Start

### Prerequisites

- .NET 10.0 SDK
- SQLite (included with .NET)

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd Task.API
```

2. Restore packages:
```bash
dotnet restore
```

3. Run database migrations:
```bash
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

The API will be available at `https://localhost:5001` and the Swagger documentation at `https://localhost:5001/scalar/v1`.

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token

### Tasks
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create a new task
- `PUT /api/tasks/{id}/status` - Update task status
- `DELETE /api/tasks/{id}` - Delete a task

## Configuration

The application uses `appsettings.json` for configuration. Key settings include:

- **Database**: Connection string for SQLite database
- **JWT**: Key, issuer, and audience for token validation
- **Logging**: Log levels for different components

## Development

### Running Tests
```bash
dotnet test
```

### Building for Production
```bash
dotnet publish -c Release
```

### Database Migrations
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or issues, please open an issue on GitHub.
