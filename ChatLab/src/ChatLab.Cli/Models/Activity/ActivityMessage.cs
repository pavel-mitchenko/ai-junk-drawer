namespace ChatLab.Cli.Models.Activity;

// A trimmed message record — just enough to drive the activity charts.
public sealed class ActivityMessage
{
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string UserId { get; set; } = string.Empty;
}
