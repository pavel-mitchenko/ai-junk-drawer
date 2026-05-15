namespace ChatLab.Cli.Models.Stats;

// A user entry in the obfuscated stats output — id is the dictionary key,
// so this carries only the hand-maintained presentation fields.
public sealed class ObfuscatedUser
{
    public string Alias { get; set; } = string.Empty;
    public string AvatarUri { get; set; } = string.Empty;
}
