using TaskList.Api.DTOs;
using TaskList.Api.Models;

namespace TaskList.Api.Services.Interfaces
{
    public interface IAiService
    {
        Task<string> GenerateTaskSummaryAsync(List<TaskItem> tasks);
        Task<DocumentAiResult> AnalyzeDocumentAsync(string documentText);
    }
}
