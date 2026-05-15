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
        var existing = await ReadExistingUsersAsync(usersPath);
        foreach (var u in users)
        {
            if (existing.TryGetValue(u.Id, out var prev))
            {
                u.Alias = prev.Alias;
                u.AvatarUri = prev.AvatarUri;
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

    private static async Task<Dictionary<string, StatsUser>> ReadExistingUsersAsync(string usersPath)
    {
        if (!File.Exists(usersPath))
        {
            return new Dictionary<string, StatsUser>();
        }
        await using var input = File.OpenRead(usersPath);
        var existing = await JsonSerializer.DeserializeAsync<List<StatsUser>>(input, StatsIO.JsonOptions);
        return existing?.ToDictionary(u => u.Id) ?? new Dictionary<string, StatsUser>();
    }
}
