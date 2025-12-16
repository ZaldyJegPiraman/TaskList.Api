using System.ComponentModel.DataAnnotations;

namespace TaskList.Api.Models
{

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        public string? Category { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;

        public int UserId { get; set; }
    }
}
