namespace Task.API.Dtos;

public class RegisterDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? FName { get; set; }
    public string? LName { get; set; }
    public int? SIN { get; set; }
    public string? University { get; set; }
}