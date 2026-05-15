using ChatLab.Cli.Models.Telegram;

namespace ChatLab.Cli.Commands;

public static class DebugCommand
{
    public static async Task RunAsync(string exportFolder)
    {
        var resultPath = Path.Combine(exportFolder, TelegramExportReader.ResultFileName);
        var export = await TelegramExportReader.LoadAsync(resultPath);

        Console.WriteLine($"Messages: {export.Messages.Count}");
        if (export.Messages.Count > 0)
        {
            var min = export.Messages.Min(m => m.Date);
            var max = export.Messages.Max(m => m.Date);
            Console.WriteLine($"Earliest: {min:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Latest:   {max:yyyy-MM-dd HH:mm:ss}");
        }

        PrintGroups("FromId", export.Messages.Select(m => m.FromId));
        PrintGroups("Type", export.Messages.Select(m => (string?)m.Type));
        PrintGroups("MediaType", export.Messages.Select(m => m.MediaType));
        PrintGroups("MimeType", export.Messages.Select(m => m.MimeType));
        PrintGroups(
            "TextEntities.Type",
            export.Messages.SelectMany(m => m.TextEntities ?? Enumerable.Empty<TelegramTextEntity>())
                           .Select(e => (string?)e.Type));
    }

    private static void PrintGroups(string label, IEnumerable<string?> values)
    {
        Console.WriteLine();
        Console.WriteLine($"{label}:");
        var groups = values
            .GroupBy(v => v ?? "(null)")
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal);

        foreach (var g in groups)
        {
            Console.WriteLine($"  {g.Key}: {g.Count()}");
        }
    }
}
