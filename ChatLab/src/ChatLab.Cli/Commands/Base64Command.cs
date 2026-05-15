using System.Buffers.Text;
using System.Text;

namespace ChatLab.Cli.Commands;

public static class Base64Command
{
    public static Task RunAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Base64 (URL-safe):");
        Console.WriteLine("  1. Encode");
        Console.WriteLine("  2. Decode");
        Console.Write("> ");
        var choice = Console.ReadLine()?.Trim();

        Console.Write("Input: ");
        var input = Console.ReadLine() ?? string.Empty;

        switch (choice)
        {
            case "1":
            {
                var encoded = Base64Url.EncodeToString(Encoding.UTF8.GetBytes(input));
                Console.WriteLine($"Encoded: {encoded}");
                break;
            }
            case "2":
            {
                var bytes = Base64Url.DecodeFromChars(input);
                var decoded = Encoding.UTF8.GetString(bytes);
                Console.WriteLine($"Decoded: {decoded}");
                break;
            }
            default:
            {
                Console.WriteLine($"Unknown choice: '{choice}'.");
                break;
            }
        }

        return Task.CompletedTask;
    }
}
