
namespace DownloadInstaller
{
    internal static class Log
    {
        public static void Error(string error)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(error);
            Console.ForegroundColor = previousColor;
        }

        public static void Info(string error)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine(error);
            Console.ForegroundColor = previousColor;
        }

        public static void Confirm(string text)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Please check the following statement\n\n{text}\n\nIf the information above is correct press <Enter>, otherwise Ctrl+C");
            Console.ForegroundColor = previousColor;
            Console.ReadLine();
        }
    }
}
