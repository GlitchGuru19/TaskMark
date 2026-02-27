# Quick Start Guide - Task.API

This guide will help you get up and running with Task.API in just a few minutes.

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 10.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Visual Studio 2022** or **VS Code** (recommended)
- **SQLite** (included with .NET)

## Step 1: Clone the Repository

```
bash
git clone <repository-url>
cd Task.API
```

## Step 2: Restore Dependencies

```
bash
dotnet restore
```

## Step 3: Setup Database

The database will be automatically created when you run the application. However, if you need to run migrations manually:

```
bash
dotnet ef database update
```

## Step 4: Run the Application

```
bash
dotnet run
```

The application will start on `https://localhost:5001` (or `http://localhost:5000`).

## Step 5: Access API Documentation

Once running, open your browser and navigate to:

- **Scalar UI**: `https://localhost:5001/scalar/v1`
- **Swagger UI**: `https://localhost:5001/swagger`

## First Steps with the API

### 1. Register a User

```
bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email": "user@example.com", "password": "YourPassword123!"}'
```

### 2. Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "user@example.com", "password": "YourPassword123!"}'
```

Copy the JWT token from the response.

### 3. Create a Task

```
bash
curl -X POST https://localhost:5001/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "title": "My First Task",
    "category": "Work",
    "categoryColor": "#007bff",
    "assignedUser": "user@example.com"
  }'
```

### 4. Get All Tasks

```bash
curl -X GET https://localhost:5001/api/tasks \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 5. Update Task Status

```
bash
curl -X PUT https://localhost:5001/api/tasks/1/status \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{"status": "InProgress"}'
```

### 6. Delete a Task

```
bash
curl -X DELETE https://localhost:5001/api/tasks/1 \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Configuration

### Default Ports

- **HTTPS**: 5001
- **HTTP**: 5000

### Database Location

The SQLite database is stored at: `Data/tasks.db`

### JWT Settings

Default JWT settings are configured in `appsettings.json`. For production, update these values:

```
json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHere",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience"
  }
}
```

## Troubleshooting

### Port Already in Use

If you get a port error, modify `Properties/launchSettings.json` to use different ports.

### Database Issues

Delete the existing database and re-run migrations:

```
bash
rm Data/tasks.db
dotnet ef database update
```

### SSL Certificate Errors

For development, you can trust the development certificate:

```
bash
dotnet dev-certs https --trust
```

## Next Steps

- Read the [Architecture](Architecture.md) documentation
- Check the [API Reference](API.md) for detailed endpoint documentation
- Explore the [Summary](Summary.md) for project overview
