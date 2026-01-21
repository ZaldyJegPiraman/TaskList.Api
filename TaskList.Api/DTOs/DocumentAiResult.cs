using TaskList.Api.Models;

namespace TaskList.Api.DTOs
{
    public class DocumentAiResult
    {
        public string Summary { get; set; } = "";
        public List<ExtractedTaskDto> Tasks { get; set; } = new();
    }

    public class AiTaskItem
    {
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }
}
