using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using ChatLab.Cli.Models.Stats;

namespace ChatLab.Cli.Commands;

public static class BuildStatsCommand
{
    private const string StatsFolderName = "stats";
    private const string RawStatsFileName = "raw.json";
    private const string RawUsersFileName = "raw-users.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        // Default encoder escapes anything non-ASCII (so "Антон" -> "А…").
        // Allow all Unicode ranges so hand-editable fields like Alias stay readable.
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, TelegramExportReader.ResultFileName);
        var export = await TelegramExportReader.LoadAsync(resultPath);

        var totalInput = export.Messages.Count;
        var nonService = export.Messages.Count(m => m.Type == "message");
        var (stats, users) = StatsBuilder.Build(export);
        var skipped = nonService - stats.Messages.Count;

        var statsDir = Path.Combine(exportFolder, StatsFolderName);
        Directory.CreateDirectory(statsDir);

        var usersPath = Path.Combine(statsDir, RawUsersFileName);
        var aliases = await ReadAliasesAsync(usersPath);
        foreach (var u in users)
        {
            if (aliases.TryGetValue(u.Id, out var alias))
            {
                u.Alias = alias;
            }
        }

        var rawPath = Path.Combine(statsDir, RawStatsFileName);
        await WriteJsonAsync(rawPath, stats);
        await WriteJsonAsync(usersPath, users);

        Console.WriteLine($"Source messages:     {totalInput}");
        Console.WriteLine($"  of which non-service: {nonService}");
        Console.WriteLine($"Users:               {users.Count}");
        Console.WriteLine($"Stats messages:      {stats.Messages.Count}");

        var byType = stats.Messages
            .GroupBy(m => m.Type)
            .OrderByDescending(g => g.Count());

        foreach (var g in byType)
        {
            Console.WriteLine($"  {g.Key}: {g.Count()}");
        }

        Console.WriteLine($"Skipped (no matching category): {skipped}");
        Console.WriteLine($"Wrote: {rawPath}");
        Console.WriteLine($"Wrote: {usersPath}");
    }

    private static async Task<Dictionary<string, string>> ReadAliasesAsync(string usersPath)
    {
        if (!File.Exists(usersPath))
        {
            return new Dictionary<string, string>();
        }
        await using var input = File.OpenRead(usersPath);
        var existing = await JsonSerializer.DeserializeAsync<List<StatsUser>>(input, JsonOptions);
        if (existing is null)
        {
            return new Dictionary<string, string>();
        }
        return existing
            .Where(u => !string.IsNullOrEmpty(u.Alias))
            .ToDictionary(u => u.Id, u => u.Alias!);
    }

    private static async Task WriteJsonAsync<T>(string path, T value)
    {
        await using var output = File.Create(path);
        await JsonSerializer.SerializeAsync(output, value, JsonOptions);
    }
}
