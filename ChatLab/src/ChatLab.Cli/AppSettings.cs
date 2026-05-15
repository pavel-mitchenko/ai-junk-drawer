using Microsoft.Extensions.Configuration;

namespace ChatLab.Cli;

// Settings shape backing appsettings.json / appsettings.Local.json.
public sealed class AppSettings
{
    public string ExportFolder { get; set; } = string.Empty;

    // Upper bound, in seconds, of the random positive jitter that the
    // obfuscator adds to each message's timestamp. Range is [0.1, value].
    // Null disables time obfuscation entirely.
    public double? TimeJitterSeconds { get; set; }

    public static AppSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false)
            .Build();

        var settings = new AppSettings();
        config.Bind(settings);
        return settings;
    }
}
