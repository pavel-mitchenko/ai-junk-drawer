using System.Text.Json;
using ChatLab.Cli.Models.Telegram;

namespace ChatLab.Cli;

public static class TelegramExportReader
{
    public const string ResultFileName = "result.json";

    public static async Task<TelegramExport> LoadAsync(string filePath)
    {
        await using var stream = File.OpenRead(filePath);

        var result =  await JsonSerializer.DeserializeAsync<TelegramExport>(stream)
            ?? throw new InvalidOperationException($"Failed to parse '{filePath}'.");

       // result.Messages = result.Messages.Take(20).ToList();

        return result;
    }
}
