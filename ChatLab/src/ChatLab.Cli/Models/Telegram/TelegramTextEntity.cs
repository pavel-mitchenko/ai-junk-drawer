using System.Text.Json.Serialization;

namespace ChatLab.Cli.Models.Telegram;

// One segment from `text_entities` — e.g. plain text, bold, or a text_link.
public sealed class TelegramTextEntity
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
