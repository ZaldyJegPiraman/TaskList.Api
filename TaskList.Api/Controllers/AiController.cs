using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Claims;
using TaskList.Api.Data;
using TaskList.Api.Services;
using TaskList.Api.Services.Interfaces;

[Authorize]
[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;
    private readonly AppDbContext _db;
    private readonly DocumentTextExtractor _documentTextExtractor;
    public AiController(IAiService aiService, AppDbContext db, DocumentTextExtractor documentTextExtractor)
    {
        _aiService = aiService;
        _db = db;
        _documentTextExtractor = documentTextExtractor;
    }


    [HttpGet("task-summary")]
    public async Task<IActionResult> GetTaskSummary()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token");

        var userId = int.Parse(userIdClaim); // ✅ FIX

        var tasks = await _db.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();

        var summary = await _aiService.GenerateTaskSummaryAsync(tasks);

        return Ok(new { summary });
    }



    [HttpPost("analyze-document")]
    public async Task<IActionResult> AnalyzeDocument(IFormFile file)
    {
        var text = await _documentTextExtractor.ExtractAsync(file);
        var result = await _aiService.AnalyzeDocumentAsync(text);

        return Ok(result);
    }
}