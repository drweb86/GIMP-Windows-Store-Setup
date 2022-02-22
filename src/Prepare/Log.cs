
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

        public static void Debug(string error)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Out.WriteLine(error);
            Console.ForegroundColor = previousColor;
        }
    }
}
