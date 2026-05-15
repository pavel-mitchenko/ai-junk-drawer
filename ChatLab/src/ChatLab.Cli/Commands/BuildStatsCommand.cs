using System.Text.Json;
using ChatLab.Cli.Models.Stats;

namespace ChatLab.Cli.Commands;

public static class BuildStatsCommand
{
    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, TelegramExportReader.ResultFileName);
        var export = await TelegramExportReader.LoadAsync(resultPath);

        var totalInput = export.Messages.Count;
        var nonService = export.Messages.Count(m => m.Type == "message");
        var (stats, users) = StatsBuilder.Build(export);
        var skipped = nonService - stats.Messages.Count;

        var statsDir = Path.Combine(exportFolder, StatsIO.FolderName);
        Directory.CreateDirectory(statsDir);

        var usersPath = Path.Combine(statsDir, StatsIO.RawUsersFileName);
        var aliases = await ReadAliasesAsync(usersPath);
        foreach (var u in users)
        {
            if (aliases.TryGetValue(u.Id, out var alias))
            {
                u.Alias = alias;
            }
        }

        var rawPath = Path.Combine(statsDir, StatsIO.RawStatsFileName);
        await StatsIO.WriteAsync(rawPath, stats);
        await StatsIO.WriteAsync(usersPath, users);

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
        var existing = await JsonSerializer.DeserializeAsync<List<StatsUser>>(input, StatsIO.JsonOptions);
        if (existing is null)
        {
            return new Dictionary<string, string>();
        }
        return existing
            .Where(u => !string.IsNullOrEmpty(u.Alias))
            .ToDictionary(u => u.Id, u => u.Alias!);
    }
}
