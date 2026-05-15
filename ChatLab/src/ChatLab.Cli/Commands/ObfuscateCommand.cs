using ChatLab.Cli.Models.Stats;

namespace ChatLab.Cli.Commands;

public static class ObfuscateCommand
{
    public static async Task RunAsync(string exportFolder, double? timeJitterSeconds, double? durationJitterSeconds)
    {
        var statsDir = Path.Combine(exportFolder, StatsIO.FolderName);
        var rawPath = Path.Combine(statsDir, StatsIO.RawStatsFileName);
        var usersPath = Path.Combine(statsDir, StatsIO.RawChatInfoFileName);
        var outPath = Path.Combine(statsDir, StatsIO.ObfuscatedStatsFileName);

        var stats = await StatsIO.ReadAsync<ChatStats>(rawPath);
        var rawChat = await StatsIO.ReadAsync<RawChatInfo>(usersPath);

        var obfuscated = StatsObfuscator.Obfuscate(stats, rawChat, timeJitterSeconds, durationJitterSeconds);
        await StatsIO.WriteAsync(outPath, obfuscated, ignoreNulls: true);

        Console.WriteLine($"Users:  {obfuscated.UsersMapping.Count}");
        Console.WriteLine($"Items:  {obfuscated.Items.Count}");
        Console.WriteLine($"Wrote: {outPath}");
    }
}
