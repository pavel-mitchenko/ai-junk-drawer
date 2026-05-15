using ChatLab.Cli.Models.Stats;

namespace ChatLab.Cli;

public static class StatsObfuscator
{
    public static ObfuscatedChatStats Obfuscate(ChatStats stats, List<StatsUser> users)
    {
        var unaliased = users.FirstOrDefault(u => string.IsNullOrEmpty(u.Alias));
        if (unaliased is not null)
        {
            throw new InvalidOperationException(
                $"User '{unaliased.Id}' has no alias filled in {StatsIO.RawUsersFileName}.");
        }

        var idMapping = new Dictionary<string, string>();
        var usersMapping = new Dictionary<string, string>();
        var counter = 1;
        foreach (var u in users)
        {
            var newId = counter.ToString();
            idMapping[u.Id] = newId;
            usersMapping[newId] = u.Alias!;
            counter++;
        }

        var items = stats.Messages
            .Select(m => new StatsMessage
            {
                Type = m.Type,
                Date = m.Date,
                UserId = idMapping[m.UserId],
                UserName = null,
                AggregatedText = null
            })
            .ToList();

        return new ObfuscatedChatStats
        {
            Items = items,
            UsersMapping = usersMapping,
        };
    }
}
