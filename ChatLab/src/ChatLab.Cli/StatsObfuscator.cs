using ChatLab.Cli.Models.Stats;

namespace ChatLab.Cli;

public static class StatsObfuscator
{
    public static ObfuscatedChatStats Obfuscate(ChatStats stats, RawChatInfo rawChat, double? timeJitterSeconds)
    {
        if (string.IsNullOrEmpty(rawChat.ChatName))
        {
            throw new InvalidOperationException(
                $"ChatName is empty in {StatsIO.RawChatInfoFileName}.");
        }

        var unaliased = rawChat.Users.FirstOrDefault(u => string.IsNullOrEmpty(u.Alias));
        if (unaliased is not null)
        {
            throw new InvalidOperationException(
                $"User '{unaliased.Id}' has no alias filled in {StatsIO.RawChatInfoFileName}.");
        }

        var idMapping = new Dictionary<string, string>();
        var usersMapping = new Dictionary<string, ObfuscatedUser>();
        var counter = 1;
        foreach (var u in rawChat.Users)
        {
            var newId = counter.ToString();
            idMapping[u.Id] = newId;
            usersMapping[newId] = new ObfuscatedUser
            {
                Alias = u.Alias,
                AvatarUri = u.AvatarUri,
            };
            counter++;
        }

        var items = stats.Messages
            .Select(m => new StatsMessage
            {
                Type = m.Type,
                Date = Jitter(m.Date, timeJitterSeconds),
                UserId = idMapping[m.UserId],
                UserName = null,
                AggregatedText = null,
                DurationSeconds = m.DurationSeconds,
            })
            .ToList();

        return new ObfuscatedChatStats
        {
            ChatName = rawChat.ChatName,
            Items = items,
            UsersMapping = usersMapping,
        };
    }

    private static DateTime Jitter(DateTime date, double? maxSeconds)
    {
        if (maxSeconds is not double max)
        {
            return date;
        }
        // Positive jitter in the range [0.1, max] seconds, fresh per call.
        const double min = 0.1;
        var seconds = min + Random.Shared.NextDouble() * (max - min);
        return date.AddSeconds(seconds);
    }
}
