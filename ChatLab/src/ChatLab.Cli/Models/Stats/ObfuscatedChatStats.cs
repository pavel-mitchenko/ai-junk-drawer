namespace ChatLab.Cli.Models.Stats;

// Final, viewer-facing stats with personal identifiers replaced
public sealed class ObfuscatedChatStats
{
    public Dictionary<string, string> UsersMapping { get; set; } = new();

    public List<StatsMessage> Items { get; set; } = new();
    
}
