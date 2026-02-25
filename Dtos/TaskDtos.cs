using Task.API.Enums;

namespace Task.API.Dtos;

public class TaskDtos
{
    public class CreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string AssignedUser { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
    }

    public class UpdateTaskStatusDto
    {
        public Status Status { get; set; }
    }

    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? AssignedUser { get; set; }
        public string? Category { get; set; }
        public string? CategoryColor { get; set; }
        public Status? Status { get; set; }

    }
}
