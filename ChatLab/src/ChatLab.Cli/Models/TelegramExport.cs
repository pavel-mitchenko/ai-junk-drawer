using System.Text.Json.Serialization;

namespace ChatLab.Cli.Models;

// Root shape of Telegram's result.json export.
public sealed class TelegramExport
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("messages")]
    public List<TelegramMessage> Messages { get; set; } = new();
}
