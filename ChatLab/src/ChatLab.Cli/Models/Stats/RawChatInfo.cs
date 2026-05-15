namespace ChatLab.Cli.Models.Stats;

public sealed class RawChatInfo
{
    public string ChatName { get; set; } = string.Empty;
    public List<StatsUser> Users { get; set; } = new();
}
