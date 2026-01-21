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



    //    public async Task<string> GenerateTaskSummaryAsync(List<TaskItem> tasks)
    //    {
    //        var today = DateTime.Today;
    //        var nextWeek = today.AddDays(7);

    //        var dueToday = tasks
    //            .Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == today)
    //            .ToList();

    //        var dueSoon = tasks
    //            .Where(t =>
    //                t.DueDate.HasValue &&
    //                t.DueDate.Value.Date > today &&
    //                t.DueDate.Value.Date <= nextWeek)
    //            .ToList();

    //        // -----------------------------
    //        // 1. Overview (AI-written)
    //        // -----------------------------
    //        var overviewPrompt = $"""
    //You are a friendly personal task assistant.

    //Write ONE short, friendly paragraph telling the user
    //they have {tasks.Count} tasks in total.

    //Do NOT list tasks.
    //Do NOT use labels.
    //""";

    //        var overview = await CallAiAsync(overviewPrompt);

    //        // -----------------------------
    //        // 2. Explain ONE task at a time
    //        // -----------------------------
    //        async Task<string> ExplainTask(TaskItem task)
    //        {
    //            var prompt = $"""
    //You are a friendly personal task assistant.

    //Explain the task below in ONE natural, conversational sentence.
    //Rephrase it like a human would.
    //Do NOT add headings, labels, or extra formatting.
    //Do NOT mention other tasks.

    //Task Title:
    //{task.Title}

    //Task Description:
    //{task.Description}
    //""";

    //            var explanation = await CallAiAsync(prompt);

    //            // 🔒 HARD CLEANUP: force single sentence
    //            return explanation
    //                .Replace("\n", " ")
    //                .Replace("•", "")
    //                .Trim();
    //        }

    //        // -----------------------------
    //        // 3. Build bullets (ONE per line)
    //        // -----------------------------
    //        var todayBullets = new List<string>();
    //        foreach (var task in dueToday)
    //        {
    //            var explanation = await ExplainTask(task);
    //            todayBullets.Add($"• **{task.Title}** — {explanation}");
    //        }

    //        var soonBullets = new List<string>();
    //        foreach (var task in dueSoon)
    //        {
    //            var explanation = await ExplainTask(task);
    //            soonBullets.Add(
    //                $"• **{task.Title}** — {explanation} (Due {task.DueDate:MMM dd})"
    //            );
    //        }

    //        // -----------------------------
    //        // 4. FINAL Markdown Output
    //        // -----------------------------
    //        return $"""
    //## Overview
    //{overview}

    //## Tasks Due Today
    //{(todayBullets.Any()
    //        ? string.Join("\n", todayBullets)
    //        : "• No tasks due today.")}

    //## Tasks Due in the Next 7 Days
    //{(soonBullets.Any()
    //        ? string.Join("\n", soonBullets)
    //        : "• No upcoming tasks.")}

    //---
    //You're doing great — keep moving forward one task at a time 💪
    //""";
    //    }
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
        // 1. Overview (AI)
        // -----------------------------
        var overviewPrompt = $"""
You are a friendly personal task assistant.

Write ONE short, friendly paragraph.
Use SECOND PERSON ("you").
Mention that the user has {tasks.Count} tasks.

STRICT RULES:
- Do NOT list tasks
- Do NOT explain what you are doing
- Do NOT use phrases like "here's", "rewritten", "this task"
""";

        var overview = await CallAiAsync(overviewPrompt);

        // -----------------------------
        // 2. Explain EACH task (AI)
        // -----------------------------
        async Task<string> ExplainTask(TaskItem task)
        {
            var prompt = $"""
You are a friendly personal task assistant.

Explain the task below in ONE natural sentence.

STRICT RULES:
- Use SECOND PERSON ("you")
- Do NOT say "I", "I'll", "we"
- Do NOT explain that you are rewriting or summarizing
- Do NOT use labels like "short sentence", "human rephrase"
- Do NOT copy text verbatim
- Output ONLY the explanation sentence

Task Title:
{task.Title}

Task Description:
{task.Description}
""";

            return await CallAiAsync(prompt);
        }

        // -----------------------------
        // 3. Build bullet lists (NEWLINE PER BULLET)
        // -----------------------------
        var todayBullets = new List<string>();
        foreach (var task in dueToday)
        {
            var explanation = await ExplainTask(task);
            todayBullets.Add($"• **{task.Title}** — {explanation}");
        }

        var soonBullets = new List<string>();
        foreach (var task in dueSoon)
        {
            var explanation = await ExplainTask(task);
            soonBullets.Add(
                $"• **{task.Title}** — {explanation} (Due {task.DueDate:MMM dd})"
            );
        }

        // -----------------------------
        // 4. Final Markdown Output
        // -----------------------------
        return $"""
## Overview
{overview}

## Tasks Due Today
{(todayBullets.Any()
        ? string.Join("\n\n", todayBullets)   // ✅ DOUBLE newline = visual separation
        : "• No tasks due today.")}

## Tasks Due in the Next 7 Days
{(soonBullets.Any()
        ? string.Join("\n\n", soonBullets)
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
