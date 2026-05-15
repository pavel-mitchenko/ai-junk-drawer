using System.Text.Json;

namespace ChatLab.Cli.Commands;

public static class BuildStatsCommand
{
    private const string StatsFolderName = "stats";
    private const string RawStatsFileName = "raw.json";

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, TelegramExportReader.ResultFileName);
        var export = await TelegramExportReader.LoadAsync(resultPath);

        var totalInput = export.Messages.Count;
        var nonService = export.Messages.Count(m => m.Type == "message");
        var stats = StatsBuilder.Build(export);
        var skipped = nonService - stats.Messages.Count;

        var statsDir = Path.Combine(exportFolder, StatsFolderName);
        Directory.CreateDirectory(statsDir);
        var outPath = Path.Combine(statsDir, RawStatsFileName);
        await using (var output = File.Create(outPath))
        {
            await JsonSerializer.SerializeAsync(output, stats, WriteOptions);
        }

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
        Console.WriteLine($"Wrote: {outPath}");
    }
}
