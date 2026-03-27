namespace Task.API.Dtos;

public class LoginDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class ForgotPasswordDto
{
    public string Email { get; set; } = default!;
}

public class ResetPasswordDto
{
    public string UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
