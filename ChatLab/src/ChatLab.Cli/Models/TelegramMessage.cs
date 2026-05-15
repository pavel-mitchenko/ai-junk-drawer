using System.Text.Json.Serialization;

namespace ChatLab.Cli.Models;

// A single entry from the `messages` array in result.json.
// id/type/date/date_unixtime are always present; everything else is optional.
public sealed class TelegramMessage
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("date_unixtime")]
    public string DateUnixtime { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("from_id")]
    public string? FromId { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }

    [JsonPropertyName("duration_seconds")]
    public int? DurationSeconds { get; set; }

    // `text` is a polymorphic, structurally complex field — Telegram emits it
    // as either a plain string or an array of mixed (string | entity-object)
    // segments. Skipped for now; the structured view lives in TextEntities.
    // [JsonPropertyName("text")]
    // public ??? Text { get; set; }

    [JsonPropertyName("text_entities")]
    public List<TelegramTextEntity>? TextEntities { get; set; }
}
