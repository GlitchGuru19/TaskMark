# API Documentation

## Overview
This document provides detailed information about all available API endpoints in the Task.API application.

## Base URL
```
http://localhost:5000/api
```

## Authentication
The API uses JWT (JSON Web Token) for authentication. Include the token in the `Authorization` header:
```
Authorization: Bearer <your_jwt_token>
```

---

## Authentication Endpoints

### 1. Register User
**Endpoint:** `POST /api/auth/register`

**Description:** Create a new user account

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "fName": "John",
  "lName": "Doe",
  "sin": 123456789,
  "university": "University of Example"
}
```

**Response (Success - 200 OK):**
```json
{
  "message": "User registered successfully",
  "email": "user@example.com"
}
```

**Response (Error - 400 Bad Request):**
```json
{
  "errors": [
    {
      "code": "PasswordTooShort",
      "description": "Passwords must be at least 6 characters."
    }
  ]
}
```

---

### 2. Login
**Endpoint:** `POST /api/auth/login`

**Description:** Authenticate user and receive JWT token

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response (Success - 200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "userId": "user-id-guid"
}
```

**Response (Error - 401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

---

### 3. Logout
**Endpoint:** `POST /api/auth/logout`

**Description:** Sign out the current user

**Authentication:** Required (Bearer Token)

**Response (Success - 200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

---

### 4. Forgot Password
**Endpoint:** `POST /api/auth/forgot-password`

**Description:** Request password reset token (token sent via email in production)

**Request Body:**
```json
{
  "email": "user@example.com"
}
```

**Response (Success - 200 OK):**
```json
{
  "message": "If an account exists with that email, a password reset link has been sent",
  "token": "reset-token-here",
  "userId": "user-id-guid"
}
```

---

### 5. Reset Password
**Endpoint:** `POST /api/auth/reset-password`

**Description:** Reset user password using reset token

**Request Body:**
```json
{
  "userId": "user-id-guid",
  "token": "reset-token-from-email",
  "newPassword": "NewSecurePass123!"
}
```

**Response (Success - 200 OK):**
```json
{
  "message": "Password reset successfully"
}
```

**Response (Error - 400 Bad Request):**
```json
{
  "message": "Invalid user",
  "errors": ["Invalid token"]
}
```

---

## Task Endpoints

### 1. Get All Tasks
**Endpoint:** `GET /api/tasks`

**Description:** Retrieve all tasks ordered by creation date (newest first)

**Response (Success - 200 OK):**
```json
[
  {
    "id": 1,
    "title": "Build API",
    "assignedUser": "john@example.com",
    "category": "Development",
    "categoryColor": "#FF5733",
    "status": 1,
    "createdAt": "2026-03-01T10:00:00Z",
    "updatedAt": "2026-03-02T15:30:00Z"
  },
  {
    "id": 2,
    "title": "Write Tests",
    "assignedUser": "jane@example.com",
    "category": "Testing",
    "categoryColor": "#33FF57",
    "status": 0,
    "createdAt": "2026-03-01T12:00:00Z",
    "updatedAt": "2026-03-01T12:00:00Z"
  }
]
```

---

### 2. Get Single Task
**Endpoint:** `GET /api/tasks/{id}`

**Description:** Retrieve a specific task by ID

**Parameters:**
- `id` (integer, required) - The task ID

**Response (Success - 200 OK):**
```json
{
  "id": 1,
  "title": "Build API",
  "assignedUser": "john@example.com",
  "category": "Development",
  "categoryColor": "#FF5733",
  "status": 1,
  "createdAt": "2026-03-01T10:00:00Z",
  "updatedAt": "2026-03-02T15:30:00Z"
}
```

**Response (Not Found - 404):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "..."
}
```

---

### 3. Create Task
**Endpoint:** `POST /api/tasks`

**Description:** Create a new task

**Request Body:**
```json
{
  "title": "Build API",
  "assignedUser": "john@example.com",
  "category": "Development",
  "categoryColor": "#FF5733"
}
```

**Response (Success - 201 Created):**
```json
{
  "id": 3,
  "title": "Build API",
  "assignedUser": "john@example.com",
  "category": "Development",
  "categoryColor": "#FF5733",
  "status": 0,
  "createdAt": "2026-03-02T16:00:00Z",
  "updatedAt": null
}
```

---

### 4. Update Task Status
**Endpoint:** `PUT /api/tasks/{id}/status`

**Description:** Update the status of a task

**Parameters:**
- `id` (integer, required) - The task ID

**Request Body:**
```json
{
  "status": 1
}
```

**Status Values:**
- `0` - Pending
- `1` - In Progress
- `2` - Done
- `3` - Backlog

**Response (Success - 200 OK):**
```json
{
  "id": 1,
  "title": "Build API",
  "assignedUser": "john@example.com",
  "category": "Development",
  "categoryColor": "#FF5733",
  "status": 1,
  "createdAt": "2026-03-01T10:00:00Z",
  "updatedAt": "2026-03-02T16:05:00Z"
}
```

---

### 5. Delete Task
**Endpoint:** `DELETE /api/tasks/{id}`

**Description:** Delete a task by ID

**Parameters:**
- `id` (integer, required) - The task ID

**Response (Success - 204 No Content):**
```
(No body returned)
```

**Response (Not Found - 404):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "..."
}
```

---

## Error Handling

All error responses follow this standard format:

```json
{
  "type": "error-type-url",
  "title": "Error Title",
  "status": 400,
  "detail": "Detailed error message",
  "traceId": "unique-trace-id"
}
```

### Common HTTP Status Codes
- **200 OK** - Request succeeded
- **201 Created** - Resource created successfully
- **204 No Content** - Request succeeded with no content to return
- **400 Bad Request** - Invalid request parameters
- **401 Unauthorized** - Authentication required or failed
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server-side error

---

## Database Schema

### Users
- `Id` (GUID) - Primary key
- `UserName` (string)
- `Email` (string, unique)
- `FName` (string, optional)
- `LName` (string, optional)
- `SIN` (int, optional)
- `University` (string, optional)
- `PasswordHash` (string)
- `EmailConfirmed` (boolean)
- Standard Identity fields (LockoutEnabled, AccessFailedCount, etc.)

### Tasks
- `Id` (int) - Primary key, auto-increment
- `Title` (string)
- `Category` (string)
- `CategoryColor` (string)
- `AssignedUser` (string)
- `Status` (enum: 0=Pending, 1=InProgress, 2=Done, 3=Backlog)
- `CreatedAt` (DateTime)
- `UpdatedAt` (DateTime, nullable)

---

## Rate Limiting
Not currently implemented. To be added in future versions.

## Versioning
Current API Version: **v1.0**

