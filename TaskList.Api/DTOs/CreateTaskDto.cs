using System.ComponentModel.DataAnnotations;
using TaskList.Api.Models;

namespace TaskList.Api.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        public string? Category { get; set; }

        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.ToDo;
    }
}
