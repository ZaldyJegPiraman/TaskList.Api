namespace TaskList.Api.DTOs
{
    public class ExtractedTaskDto
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        public List<string> People { get; set; } = new();

    }
}
