using ChatLab.Cli.Models.Stats;
using ChatLab.Cli.Models.Telegram;

namespace ChatLab.Cli;

public static class StatsBuilder
{
    // Categories the stats report tracks. Anything outside these is dropped.
    private const string VideoMessage = "video_message";
    private const string VoiceMessage = "voice_message";
    private const string Text = "text";

    public static (ChatStats Stats, List<StatsUser> Users) Build(TelegramExport export)
    {
        var realMessages = export.Messages
            .Where(m => m.Type == "message")
            .ToList();

        var orphan = realMessages.FirstOrDefault(m => m.FromId is null);
        if (orphan is not null)
        {
            throw new InvalidOperationException($"Message {orphan.Id} has type=message but no from_id.");
        }

        var users = realMessages
            .GroupBy(m => m.FromId!)
            .Select(g => new StatsUser
            {
                Id = g.Key,
                Name = g.Select(m => m.From).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? string.Empty,
            })
            .ToList();

        var messages = new List<StatsMessage>();
        foreach (var m in realMessages)
        {
            var resolved = ResolveType(m);
            if (resolved is null)
            {
                continue;
            }
            messages.Add(new StatsMessage
            {
                Type = resolved,
                Date = m.Date,
                UserId = m.FromId!,
                UserName = m.From,
            });
        }

        return (new ChatStats { Messages = messages }, users);
    }

    private static string? ResolveType(TelegramMessage m) => m.MediaType switch
    {
        VideoMessage => VideoMessage,
        VoiceMessage => VoiceMessage,
        _ when m.TextEntities is { Count: > 0 } => Text,
        _ => null,
    };
}
