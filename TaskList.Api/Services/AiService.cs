using System.Net.Http.Json;
using System.Text.Json;
using TaskList.Api.DTOs;
using TaskList.Api.Models;
using TaskList.Api.Services.Interfaces;

public class AiService : IAiService
{
    private readonly HttpClient _http;

    public AiService(HttpClient http)
    {
        _http = http;
        _http.Timeout = TimeSpan.FromSeconds(60);
    }

    public async Task<string> GenerateTaskSummaryAsync(List<TaskItem> tasks)
    {
        var today = DateTime.Today;
        var nextWeek = today.AddDays(7);

        var dueToday = tasks
            .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today)
            .ToList();

        var dueSoon = tasks
            .Where(t =>
                t.DueDate.HasValue &&
                t.DueDate.Value.Date > today &&
                t.DueDate.Value.Date <= nextWeek)
            .ToList();

        // -----------------------------
        // 1. Overview (AI-written)
        // -----------------------------
        var overviewPrompt = $"""
You are a friendly personal task assistant.

Write ONE short, friendly overview paragraph
mentioning that the user has {tasks.Count} total tasks.

Do NOT list tasks.
""";

        var overview = await CallAiAsync(overviewPrompt);

        // -----------------------------
        // 2. Explain each task (AI)
        // -----------------------------
        async Task<string> ExplainTask(TaskItem task)
        {
            var prompt = $"""
You are a friendly personal task assistant.

Explain this task in ONE short, natural sentence.
Rephrase it in a human, conversational way.
Do NOT copy text verbatim.

Task Title:
{task.Title}

Task Description:
{task.Description}
""";

            return await CallAiAsync(prompt);
        }

        var todayExplanations = new List<string>();
        foreach (var task in dueToday)
        {
            var explanation = await ExplainTask(task);
            todayExplanations.Add($"• **{task.Title}** — {explanation}");
        }

        var soonExplanations = new List<string>();
        foreach (var task in dueSoon)
        {
            var explanation = await ExplainTask(task);
            soonExplanations.Add(
                $"• **{task.Title}** — {explanation} (Due {task.DueDate:MMM dd})"
            );
        }

        // -----------------------------
        // 3. Assemble FINAL markdown
        // -----------------------------
        return $"""
## Overview
{overview}

## Tasks Due Today
{(todayExplanations.Any()
        ? string.Join("\n", todayExplanations)
        : "• No tasks due today.")}

## Tasks Due in the Next 7 Days
{(soonExplanations.Any()
        ? string.Join("\n", soonExplanations)
        : "• No upcoming tasks.")}

---
You're doing great — keep moving forward one task at a time 💪
""";
    }


    private async Task<string> CallAiAsync(string prompt)
    {
        var payload = new
        {
            model = "llama3:8b-instruct-q4_K_M",
            prompt,
            stream = false
        };

        var response = await _http.PostAsJsonAsync("/api/generate", payload);
        response.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("response").GetString()!;
    }

    // =========================
    // ✅ DOCUMENT ANALYSIS
    // =========================
    public async Task<DocumentAiResult> AnalyzeDocumentAsync(string documentText)
    {
        var prompt = $@"
You are an information extraction engine.

Return ONLY valid JSON.
Do NOT include markdown, comments, or explanations.

JSON FORMAT:
{{
  ""summary"": ""short summary"",
  ""tasks"": [
    {{
      ""title"": ""task title"",
      ""description"": ""task description"",
      ""dueDate"": ""YYYY-MM-DD or null"",
      ""people"": [""name or email""]
    }}
  ]
}}

RULES:
- Extract real tasks only.
- Convert dates to ISO format.
- If no tasks found, return an empty array.

DOCUMENT TEXT:
{documentText}
";

        var payload = new
        {
            model = "llama3:8b-instruct-q4_K_M",
            prompt,
            temperature = 0, // ✅ critical for JSON stability
            stream = false
        };

        var response = await _http.PostAsJsonAsync("/api/generate", payload);
        response.EnsureSuccessStatusCode();

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var aiText = json.RootElement.GetProperty("response").GetString();

        try
        {
            return JsonSerializer.Deserialize<DocumentAiResult>(
                aiText!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new DocumentAiResult();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "AI returned invalid JSON:\n" + aiText, ex);
        }
    }
}
