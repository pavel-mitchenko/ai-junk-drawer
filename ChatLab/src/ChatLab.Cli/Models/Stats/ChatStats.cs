namespace ChatLab.Cli.Models.Stats;

// Viewer-facing chat stats derived from a Telegram export.
public sealed class ChatStats
{
    public List<StatsUser> Users { get; set; } = new();
    public List<StatsMessage> Messages { get; set; } = new();
}
