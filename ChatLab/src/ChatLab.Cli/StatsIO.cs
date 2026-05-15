using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace ChatLab.Cli;

internal static class StatsIO
{
    public const string FolderName = "stats";
    public const string RawStatsFileName = "raw.json";
    public const string RawChatInfoFileName = "raw-chat-info.json";
    public const string ObfuscatedStatsFileName = "obfuscated-stats.json";

    public static readonly JsonSerializerOptions JsonOptions = CreateOptions(ignoreNulls: false);
    private static readonly JsonSerializerOptions JsonOptionsIgnoreNulls = CreateOptions(ignoreNulls: true);

    public static async Task<T> ReadAsync<T>(string path)
    {
        await using var input = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(input, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to parse '{path}'.");
    }

    public static async Task WriteAsync<T>(string path, T value, bool ignoreNulls = false)
    {
        await using var output = File.Create(path);
        var options = ignoreNulls ? JsonOptionsIgnoreNulls : JsonOptions;
        await JsonSerializer.SerializeAsync(output, value, options);
    }

    private static JsonSerializerOptions CreateOptions(bool ignoreNulls)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        };
        if (ignoreNulls)
        {
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        }
        return options;
    }
}
