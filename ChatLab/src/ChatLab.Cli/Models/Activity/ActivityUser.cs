namespace ChatLab.Cli.Models.Activity;

// One distinct chat participant. Id is stable; Name is the first display
// name seen for that id (display names can change over the chat's lifetime).
public sealed class ActivityUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
