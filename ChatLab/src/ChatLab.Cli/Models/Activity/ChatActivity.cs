namespace ChatLab.Cli.Models.Activity;

// Viewer-facing activity report derived from a Telegram export.
public sealed class ChatActivity
{
    public List<ActivityUser> Users { get; set; } = new();
    public List<ActivityMessage> Messages { get; set; } = new();
}
