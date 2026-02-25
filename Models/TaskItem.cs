using System;
using Task.API.Enums;

namespace Task.API.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? CategoryColor { get; set; }
    public string? AssignedUser  { get; set; }
    public Status? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
