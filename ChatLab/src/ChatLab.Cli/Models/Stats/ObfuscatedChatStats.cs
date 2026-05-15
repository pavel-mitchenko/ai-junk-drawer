namespace ChatLab.Cli.Models.Stats;

// Final, viewer-facing stats with personal identifiers replaced
public sealed class ObfuscatedChatStats
{
    public string ChatName { get; set; } = string.Empty;

    public Dictionary<string, ObfuscatedUser> UsersMapping { get; set; } = new();

    public List<StatsMessage> Items { get; set; } = new();

}
