using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task.API.Models;
using Task.API.Dtos;

namespace Task.API.Controllers;

/// <summary>
/// Authentication Controller
/// 
/// This controller handles user registration endpoint only.
/// For complete documentation on all authentication endpoints (Login, Logout, Forgot Password, Reset Password),
/// please refer to Auth.md file in the root directory.
/// 
/// Authentication Flow:
/// 1. User registers with email and password
/// 2. Password is validated (6+ chars, must contain digit)
/// 3. Password is hashed (bcrypt) for secure storage
/// 4. User profile information is stored
/// 
/// For other authentication operations, see Auth.md documentation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// UserManager is responsible for user CRUD operations and password management.
    /// Provided by ASP.NET Core Identity framework.
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Constructor - Dependency Injection
    /// The UserManager is injected by ASP.NET Core's DI container
    /// </summary>
    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Register a new user account
    /// 
    /// HTTP Method: POST
    /// Route: /api/auth/register
    /// Authentication: Not required
    /// 
    /// This endpoint creates a new user account with validation and secure password hashing.
    /// </summary>
    /// <param name="dto">RegisterDto containing email, password, and optional profile information</param>
    /// <returns>Success message with user email or validation errors</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // STEP 1: Create a new ApplicationUser object with the provided information
        // We map the DTO data to the database model
        var user = new ApplicationUser
        {
            // Use email as the unique username identifier
            UserName = dto.Email,
            
            // Store email for authentication and communication
            Email = dto.Email,
            
            // Optional profile information
            FName = dto.FName,          // First name (can be null)
            LName = dto.LName,          // Last name (can be null)
            SIN = dto.SIN,              // Social Insurance Number (can be null)
            University = dto.University  // University affiliation (can be null)
        };

        // STEP 2: Create the user using UserManager
        // UserManager automatically:
        // - Validates password complexity (6+ chars, must have digit)
        // - Checks if email already exists (email must be unique)
        // - HASHES the password using bcrypt (NEVER stores plain text)
        // - Saves user to database
        var result = await _userManager.CreateAsync(user, dto.Password);

        // STEP 3: Check if user creation was successful
        // If any validation failed, result.Succeeded will be false
        if (!result.Succeeded)
        {
            // Return 400 Bad Request with detailed error information
            // Errors might include:
            // - "Passwords must be at least 6 characters"
            // - "Passwords must have at least one digit"
            // - "Email 'user@example.com' is already taken"
            return BadRequest(result.Errors);
        }

        // STEP 4: Success! User was created and saved to database
        // Return 200 OK with confirmation message and user email
        return Ok(new 
        { 
            message = "User registered successfully", 
            email = user.Email 
        });
    }

    // ============================================================================
    // NOTE: For additional authentication endpoints, see Auth.md documentation:
    // ============================================================================
    // - POST /api/auth/login           → Authenticate and get JWT token
    // - POST /api/auth/logout          → Sign out authenticated user
    // - POST /api/auth/forgot-password → Request password reset token
    // - POST /api/auth/reset-password  → Reset password with token
    // 
    // These endpoints are fully documented in Auth.md with:
    // - Complete code implementation
    // - Request/response examples
    // - Error scenarios
    // - Security notes
    // ============================================================================
}