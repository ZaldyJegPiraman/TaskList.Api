using TaskList.Api.Models;

namespace TaskList.Api.DTOs
{
    public class TaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
