namespace ChatLab.Cli.Commands;

public static class BuildStatsCommand
{
    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, "result.json");
        var export = await TelegramExportReader.LoadAsync(resultPath);

        var totalInput = export.Messages.Count;
        var nonService = export.Messages.Count(m => m.Type == "message");
        var stats = StatsBuilder.Build(export);
        var skipped = nonService - stats.Messages.Count;

        Console.WriteLine($"Source messages:     {totalInput}");
        Console.WriteLine($"  of which non-service: {nonService}");
        Console.WriteLine($"Users:               {stats.Users.Count}");
        Console.WriteLine($"Stats messages:      {stats.Messages.Count}");

        var byType = stats.Messages
            .GroupBy(m => m.Type)
            .OrderByDescending(g => g.Count());

        foreach (var g in byType)
        {
            Console.WriteLine($"  {g.Key}: {g.Count()}");
        }

        Console.WriteLine($"Skipped (no matching category): {skipped}");
    }
}
