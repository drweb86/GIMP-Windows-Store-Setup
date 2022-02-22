using System.Diagnostics;
using System.Reflection;

namespace Launcher.FullTrust
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var appFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Process.Start(@"C:\Users\Administrator\source\repos\drweb86\GIMP-Windows-Store-Setup\src\temp\install-setup\bin\gimp-2.10.exe");
            //var gimpFolder = new DirectoryInfo(Path.Combine(appFolder, "..", "app", "bin")).FullName;
            //var gimp = Directory
            //    .GetFiles(gimpFolder, "gimp-*.*.exe")
            //    .OrderBy(x => x.Length)
            //    .First();
            //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(gimp) { WorkingDirectory = gimpFolder });
        }
    }
}