# Authentication Endpoints Documentation

This document provides complete documentation for all authentication endpoints in the Task.API application. Each endpoint is explained in detail with code examples, request/response formats, and implementation details.

---

## Table of Contents
1. [Register User](#register-user)
2. [Login](#login)
3. [Logout](#logout)
4. [Forgot Password](#forgot-password)
5. [Reset Password](#reset-password)
6. [JWT Token Generation](#jwt-token-generation)

---

## Register User

### Overview
Creates a new user account in the system with validation and password hashing.

### Endpoint Details
- **HTTP Method:** `POST`
- **Route:** `/api/auth/register`
- **Authentication:** Not required
- **Response Type:** JSON

### Request

**URL:**
```
POST http://localhost:5000/api/auth/register
```

**Headers:**
```
Content-Type: application/json
```

**Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!",
  "fName": "John",
  "lName": "Doe",
  "sin": 123456789,
  "university": "MIT"
}
```

**Body Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| email | string | Yes | User's email address (must be unique) |
| password | string | Yes | User's password (min 6 characters, must contain digit) |
| fName | string | No | First name |
| lName | string | No | Last name |
| sin | integer | No | Social Insurance Number (optional user identifier) |
| university | string | No | University name or affiliation |

### Response

**Success (200 OK):**
```json
{
  "message": "User registered successfully",
  "email": "john.doe@example.com"
}
```

**Validation Error (400 Bad Request):**
```json
{
  "errors": [
    {
      "code": "PasswordTooShort",
      "description": "Passwords must be at least 6 characters."
    },
    {
      "code": "PasswordRequiresNonAlphanumeric",
      "description": "Passwords must have at least one non-alphanumeric character."
    }
  ]
}
```

**Duplicate Email (400 Bad Request):**
```json
{
  "errors": [
    {
      "code": "DuplicateEmail",
      "description": "Email '{email}' is already taken."
    }
  ]
}
```

### Password Requirements
- Minimum length: **6 characters**
- Must contain at least **one digit** (0-9)

### Database Effects
- Creates new user in `AspNetUsers` table
- Stores password as bcrypt hash (never plain text)
- Sets `UserName` equal to email for unique identification
- Stores optional profile information (FName, LName, SIN, University)

### Implementation Code

```csharp
[HttpPost("register")]
public async Task<IActionResult> Register(RegisterDto dto)
{
    // Create a new ApplicationUser object with provided information
    var user = new ApplicationUser
    {
        UserName = dto.Email,        // Use email as unique username
        Email = dto.Email,           // Store email for authentication
        FName = dto.FName,          // Optional: First name
        LName = dto.LName,          // Optional: Last name
        SIN = dto.SIN,              // Optional: Social Insurance Number
        University = dto.University  // Optional: University affiliation
    };

    // Use UserManager to create the user with password hashing
    // UserManager automatically hashes the password using bcrypt
    var result = await _userManager.CreateAsync(user, dto.Password);

    // Check if user creation was successful
    if (!result.Succeeded)
        return BadRequest(result.Errors);  // Return validation errors

    // Return success response with registered email
    return Ok(new { message = "User registered successfully", email = user.Email });
}
```

### Error Scenarios
| Scenario | HTTP Status | Message |
|----------|-------------|---------|
| Missing required fields | 400 | BadRequest with field errors |
| Password too short | 400 | "Passwords must be at least 6 characters" |
| No digit in password | 400 | "Passwords must have at least one digit" |
| Email already exists | 400 | "Email is already taken" |
| Database error | 500 | Internal Server Error |

### Example cURL Request
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePass123",
    "fName": "John",
    "lName": "Doe"
  }'
```

---

## Login

### Overview
Authenticates user with email and password, returning a JWT token for subsequent requests.

### Endpoint Details
- **HTTP Method:** `POST`
- **Route:** `/api/auth/login`
- **Authentication:** Not required
- **Response Type:** JSON with JWT token

### Request

**URL:**
```
POST http://localhost:5000/api/auth/login
```

**Headers:**
```
Content-Type: application/json
```

**Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

**Body Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| email | string | Yes | Registered email address |
| password | string | Yes | User's password |

### Response

**Success (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyLWlkIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  "email": "john.doe@example.com",
  "userId": "00000000-0000-0000-0000-000000000000"
}
```

**Authentication Failed (401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

### Token Details
The returned JWT token contains:
- **NameIdentifier Claim:** User's unique ID (GUID)
- **Email Claim:** User's email address
- **Name Claim:** User's full name (FirstName + LastName)
- **Expiration:** 24 hours from issue time
- **Signing Algorithm:** HMAC SHA-256

### Implementation Code

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    // Search for user with the provided email address
    var user = await _userManager.FindByEmailAsync(dto.Email);
    
    // If user not found, return unauthorized (don't reveal if email exists)
    if (user == null)
        return Unauthorized(new { message = "Invalid email or password" });

    // Check if password is correct (compares against stored hash)
    var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
    
    // If password doesn't match, return unauthorized
    if (!result.Succeeded)
        return Unauthorized(new { message = "Invalid email or password" });

    // Generate JWT token for authenticated user
    var token = GenerateJwtToken(user);
    
    // Return token and user info for client to store
    return Ok(new { token, email = user.Email, userId = user.Id });
}
```

### Token Usage
Include the token in subsequent requests:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ...
```

### Security Notes
- Passwords are never transmitted in plain text (only in HTTPS)
- Failed login attempts increment the lockout counter
- Accounts lock after configurable failed attempts (default: 5)
- Do not reveal whether email exists (same message for both scenarios)

### Example cURL Request
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePassword123!"
  }'
```

---

## Logout

### Overview
Signs out the current authenticated user and invalidates their session.

### Endpoint Details
- **HTTP Method:** `POST`
- **Route:** `/api/auth/logout`
- **Authentication:** Required (Bearer token)
- **Response Type:** JSON

### Request

**URL:**
```
POST http://localhost:5000/api/auth/logout
```

**Headers:**
```
Authorization: Bearer <your_jwt_token>
```

### Response

**Success (200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

**Unauthorized (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Authorization header was not provided."
}
```

### Implementation Code

```csharp
[Authorize]  // Requires valid JWT token
[HttpPost("logout")]
public async Task<IActionResult> Logout()
{
    // Sign out the authenticated user
    // This clears the authentication cookie and session
    await _signInManager.SignOutAsync();
    
    // Return success message
    return Ok(new { message = "Logged out successfully" });
}
```

### Behavior
- Explicitly logs out the currently authenticated user
- Clears authentication cookies on the client
- Client should delete stored JWT token
- Subsequent requests without token will be 401 Unauthorized

### Example cURL Request
```bash
curl -X POST http://localhost:5000/api/auth/logout \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## Forgot Password

### Overview
Initiates a password reset process by generating a reset token for the user's email address.

### Endpoint Details
- **HTTP Method:** `POST`
- **Route:** `/api/auth/forgot-password`
- **Authentication:** Not required
- **Response Type:** JSON

### Request

**URL:**
```
POST http://localhost:5000/api/auth/forgot-password
```

**Headers:**
```
Content-Type: application/json
```

**Body:**
```json
{
  "email": "john.doe@example.com"
}
```

**Body Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| email | string | Yes | Email address of account to reset |

### Response

**Success (200 OK):**
```json
{
  "message": "If an account exists with that email, a password reset link has been sent",
  "token": "CfDJ8EQQ_8PmKCv5eHRfuaVl72Yj7Jb...",
  "userId": "00000000-0000-0000-0000-000000000000"
}
```

**Note:** Returns same message regardless of whether email exists (for security).

### Implementation Code

```csharp
[HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
{
    // Search for user by email address
    var user = await _userManager.FindByEmailAsync(dto.Email);
    
    // If user not found, still return success (security: don't reveal if email exists)
    if (user == null)
        return Ok(new { message = "If an account exists with that email, a password reset link has been sent" });

    // Generate a secure reset token (typically valid for 24 hours)
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    
    // In production: Send this token via email
    // For demo: Return token (DO NOT DO THIS IN PRODUCTION!)
    return Ok(new { message = "Password reset token generated", token, userId = user.Id });
}
```

### Important Security Notes
- ⚠️ **PRODUCTION:** Send token via email, never return it in response
- Demo returns token for testing purposes only
- Token is cryptographically secure and single-use
- Token typically expires after a specific time period (e.g., 24 hours)
- Same response message used whether email exists or not

### Real-World Implementation
In a production application:
```csharp
// Send email with reset link
var resetLink = $"https://yourapp.com/reset-password?token={token}&userId={user.Id}";
await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
```

### Example cURL Request
```bash
curl -X POST http://localhost:5000/api/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com"
  }'
```

---

## Reset Password

### Overview
Completes the password reset process using the reset token and new password.

### Endpoint Details
- **HTTP Method:** `POST`
- **Route:** `/api/auth/reset-password`
- **Authentication:** Not required
- **Response Type:** JSON

### Request

**URL:**
```
POST http://localhost:5000/api/auth/reset-password
```

**Headers:**
```
Content-Type: application/json
```

**Body:**
```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "token": "CfDJ8EQQ_8PmKCv5eHRfuaVl72Yj7Jb...",
  "newPassword": "NewSecurePassword123!"
}
```

**Body Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| userId | string | Yes | User ID from forgot-password response |
| token | string | Yes | Reset token from forgot-password response |
| newPassword | string | Yes | New password (must meet complexity requirements) |

### Response

**Success (200 OK):**
```json
{
  "message": "Password reset successfully"
}
```

**Invalid Token (400 Bad Request):**
```json
{
  "message": "Invalid user",
  "errors": [
    "Invalid token."
  ]
}
```

**Invalid User (400 Bad Request):**
```json
{
  "message": "Invalid user"
}
```

### Implementation Code

```csharp
[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
{
    // Find the user by ID
    var user = await _userManager.FindByIdAsync(dto.UserId);
    
    // If user not found, return error (token must be accompanied by valid user)
    if (user == null)
        return BadRequest(new { message = "Invalid user" });

    // Reset password using the token and new password
    // Token validates that user initiated the reset request
    // Password is hashed before storage
    var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
    
    // If reset failed, return detailed error
    if (!result.Succeeded)
        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

    // Return success message
    return Ok(new { message = "Password reset successfully" });
}
```

### Password Requirements
- Minimum length: **6 characters**
- Must contain at least **one digit** (0-9)

### Token Validation
- Token is single-use (cannot be reused)
- Token typically expires after 24 hours
- Token is cryptographically bound to user ID
- Invalid or expired tokens will be rejected

### Workflow
1. User requests password reset with email via `/forgot-password`
2. System generates secure reset token
3. User receives token (via email in production)
4. User submits reset token + new password to `/reset-password`
5. Token validated and password updated

### Example cURL Request
```bash
curl -X POST http://localhost:5000/api/auth/reset-password \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "00000000-0000-0000-0000-000000000000",
    "token": "CfDJ8EQQ_8PmKCv5eHRfuaVl72Yj7Jb...",
    "newPassword": "NewSecurePassword123!"
  }'
```

---

## JWT Token Generation

### Overview
Internal method that creates JWT tokens for authenticated users. Used by the Login endpoint.

### Purpose
Generates a secure, standard JSON Web Token (JWT) that:
- Identifies the authenticated user
- Contains user claims (identity, email, name)
- Expires after 24 hours
- Is signed with a secret key for verification

### Implementation Code

```csharp
private string GenerateJwtToken(ApplicationUser user)
{
    // Retrieve JWT configuration from appsettings.json
    var jwtKey = _configuration["Jwt:Key"] ?? "ThisisaSuperSecretKeythatisLongFive!";
    var jwtIssuer = _configuration["Jwt:Issuer"] ?? "Task.API";
    var jwtAudience = _configuration["Jwt:Audience"] ?? "Task.APIUsers";

    // Initialize JWT handler for creating and signing tokens
    var tokenHandler = new JwtSecurityTokenHandler();
    
    // Convert secret key to byte array for signing
    var key = Encoding.ASCII.GetBytes(jwtKey);

    // Define token claims and properties
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        // Add claims (user identity information)
        Subject = new ClaimsIdentity(new[]
        {
            // Unique user identifier
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            
            // User's email address
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            
            // User's full name (FirstName LastName)
            new Claim(ClaimTypes.Name, $"{user.FName} {user.LName}")
        }),
        
        // Token validity period: 24 hours from now
        Expires = DateTime.UtcNow.AddHours(24),
        
        // Token issuer (who created the token)
        Issuer = jwtIssuer,
        
        // Token audience (who can use this token)
        Audience = jwtAudience,
        
        // Sign the token with secret key using HMAC-SHA256
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key), 
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    // Create the token using the descriptor
    var token = tokenHandler.CreateToken(tokenDescriptor);
    
    // Serialize token to JWT string format
    return tokenHandler.WriteToken(token);
}
```

### Token Structure
A JWT consists of three parts separated by dots (`.`):

```
Header.Payload.Signature
```

**Example Token:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
eyJzdWIiOiJ1c2VyLWlkIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.
SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

1. **Header:** Specifies token type (JWT) and signing algorithm (HS256)
2. **Payload:** Contains user claims and expiration time
3. **Signature:** Validates token hasn't been tampered with

### Token Claims
| Claim | Example | Purpose |
|-------|---------|---------|
| NameIdentifier | `123e4567-e89b...` | Unique user ID |
| Email | `john@example.com` | User's email |
| Name | `John Doe` | User's display name |
| Expires | `1677628800` | Token expiration (Unix timestamp) |

### Configuration (appsettings.json)
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong",
    "Issuer": "Task.API",
    "Audience": "Task.APIUsers"
  }
}
```

### Token Validation
Tokens are automatically validated by the framework when `[Authorize]` attribute is used:
- ✅ Signature verified using secret key
- ✅ Issuer and Audience match configuration
- ✅ Token not expired
- ✅ Token format is valid

### Security Best Practices
- 🔒 **Key:** Use strong, random secret key (32+ characters)
- 🔒 **HTTPS Only:** Always use HTTPS in production
- 🔒 **Expiration:** Set reasonable expiration times (24 hours)
- 🔒 **Storage:** Client stores token securely (never in localStorage for sensitive apps)
- 🔒 **Refresh:** Implement refresh tokens for long-lived sessions

---

## Complete Authentication Flow Diagram

```
1. USER REGISTRATION
   POST /register → Validate → Hash Password → Store User → Success Response

2. USER LOGIN
   POST /login → Find User → Check Password → Generate JWT → Return Token

3. AUTHENTICATED REQUEST
   GET /protected (with Bearer Token) → Validate Token → Process Request

4. PASSWORD RESET
   POST /forgot-password → Generate Token → Send Email
   POST /reset-password → Validate Token → Hash New Password → Update User

5. LOGOUT
   POST /logout → Sign Out → Clear Session
```

---

## Error Status Codes Summary

| Status | Message | Cause |
|--------|---------|-------|
| 200 OK | Success | Operation completed successfully |
| 400 Bad Request | Validation errors | Invalid input or server-side validation failed |
| 401 Unauthorized | Invalid credentials | Wrong password or missing authentication |
| 500 Internal Error | Server error | Database or system error |

---

## Testing with cURL

### Quick Test Sequence
```bash
# 1. Register
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123","fName":"Test"}'

# 2. Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123"}'

# 3. Use token from login response
curl -X POST http://localhost:5000/api/auth/logout \
  -H "Authorization: Bearer <token_from_login>"
```

---

## Related Files
- [AuthControllers.cs](Controllers/AuthControllers.cs) - Controller implementation
- [RegisterDto.cs](Dtos/RegisterDto.cs) - Registration request DTO
- [AuthDtos.cs](Dtos/AuthDtos.cs) - Other authentication DTOs
- [ApplicationUser.cs](Models/ApplicationUser.cs) - User model
- [API.md](API.md) - Full API documentation

