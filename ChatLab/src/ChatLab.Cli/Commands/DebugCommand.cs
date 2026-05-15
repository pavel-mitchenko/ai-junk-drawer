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

        PrintFromIdGroups(export.Messages);
        PrintGroups("Type", export.Messages.Select(m => (string?)m.Type));
        PrintGroups("MediaType", export.Messages.Select(m => m.MediaType));
        PrintGroups("MimeType", export.Messages.Select(m => m.MimeType));
        PrintGroups(
            "TextEntities.Type",
            export.Messages.SelectMany(m => m.TextEntities ?? Enumerable.Empty<TelegramTextEntity>())
                           .Select(e => (string?)e.Type));

        PrintDurationSanity("voice_message", export.Messages);
        PrintDurationSanity("video_message", export.Messages);

        PrintByUserMediaType(export.Messages);
    }

    private static void PrintFromIdGroups(IEnumerable<TelegramMessage> messages)
    {
        var groups = messages
            .GroupBy(m => m.FromId ?? "(null)")
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal);

        Console.WriteLine();
        Console.WriteLine("FromId:");
        foreach (var g in groups)
        {
            var name = g.Select(m => m.From).FirstOrDefault(n => !string.IsNullOrEmpty(n));
            var label = name is null ? g.Key : $"{g.Key} ({name})";
            Console.WriteLine($"  {label}: {g.Count()}");
        }
    }

    private static void PrintByUserMediaType(IEnumerable<TelegramMessage> messages)
    {
        var byUser = messages
            .Where(m => m.FromId is not null)
            .GroupBy(m => m.FromId!)
            .OrderByDescending(g => g.Count());

        Console.WriteLine();
        Console.WriteLine("By user / MediaType:");
        foreach (var userGroup in byUser)
        {
            var name = userGroup.Select(m => m.From).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? "(no name)";
            Console.WriteLine($"  {name}:");

            var byMedia = userGroup
                .GroupBy(m => m.MediaType ?? "(null)")
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key, StringComparer.Ordinal);
            foreach (var mg in byMedia)
            {
                Console.WriteLine($"    {mg.Key}: {mg.Count()}");
            }
        }
    }

    private static void PrintDurationSanity(string mediaType, IEnumerable<TelegramMessage> messages)
    {
        var media = messages.Where(m => m.MediaType == mediaType).ToList();
        var missing = media.Count(m => m.DurationSeconds is null);
        var zero = media.Count(m => m.DurationSeconds == 0);

        Console.WriteLine();
        Console.WriteLine($"{mediaType} duration:");
        Console.WriteLine($"  total:   {media.Count}");
        Console.WriteLine($"  missing: {missing}");
        Console.WriteLine($"  zero:    {zero}");
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
