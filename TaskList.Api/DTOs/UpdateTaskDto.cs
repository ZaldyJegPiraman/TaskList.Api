namespace TaskList.Api.DTOs
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public string? Category { get; set; }
        public int Status { get; set; }
    }
}
