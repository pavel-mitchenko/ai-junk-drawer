namespace ChatLab.Cli.Models.Stats;

// One distinct chat participant.
//   Id        — stable Telegram user id.
//   Name      — first display name seen for that id (display names can change).
//   Alias     — manually maintained nickname.
//   AvatarUri — manually maintained avatar link.
// Alias and AvatarUri are preserved across rebuilds by reading the previous
// raw-users file before writing the new one.
public sealed class StatsUser
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string AvatarUri { get; set; } = string.Empty;
}
