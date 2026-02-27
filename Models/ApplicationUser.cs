using Microsoft.AspNetCore.Identity;

namespace Task.API.Models;

public class ApplicationUser : IdentityUser
{
    public string? FName { get; set; }
    public string? LName { get; set; }
    public int? SIN { get; set; }
    public string? University { get; set; }
}
