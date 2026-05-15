using ChatLab.Cli;
using ChatLab.Cli.Commands;

var settings = AppSettings.Load();

Console.WriteLine($"Export directory: {settings.ExportFolder}");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Choose a command:");
    Console.WriteLine("  1. Debug");
    Console.WriteLine("  2. Build stats");
    Console.WriteLine("  3. Exit");
    Console.Write("> ");
    var input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            await DebugCommand.RunAsync(settings.ExportFolder);
            break;
        case "2":
            await BuildStatsCommand.RunAsync(settings.ExportFolder);
            break;
        case "3":
            return;
        default:
            Console.WriteLine($"Unknown command: '{input}'.");
            continue;
    }

    Console.WriteLine();
    Console.WriteLine("Done.");
}
