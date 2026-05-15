namespace ChatLab.Cli.Commands;

public static class BuildActivityCommand
{
    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, "result.json");
        var export = await TelegramExportReader.LoadAsync(resultPath);

        var totalInput = export.Messages.Count;
        var nonService = export.Messages.Count(m => m.Type == "message");
        var activity = ActivityBuilder.Build(export);
        var skipped = nonService - activity.Messages.Count;

        Console.WriteLine($"Source messages:     {totalInput}");
        Console.WriteLine($"  of which non-service: {nonService}");
        Console.WriteLine($"Users:               {activity.Users.Count}");
        Console.WriteLine($"Activity messages:   {activity.Messages.Count}");

        var byType = activity.Messages
            .GroupBy(m => m.Type)
            .OrderByDescending(g => g.Count());

        foreach (var g in byType)
        {
            Console.WriteLine($"  {g.Key}: {g.Count()}");
        }

        Console.WriteLine($"Skipped (no matching category): {skipped}");
    }
}
