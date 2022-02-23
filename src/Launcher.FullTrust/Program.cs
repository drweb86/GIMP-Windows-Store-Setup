using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

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
            var archive = Path.Combine(appFolder, "gimp-binaries.zip");

            var destinationFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(archive));

            if (!VerifyFiles(destinationFolder, appFolder))
            {
                RemoveFolder(destinationFolder);
                Directory.CreateDirectory(destinationFolder);
                ZipFile.ExtractToDirectory(archive, destinationFolder);
            }

            Execute(destinationFolder, appFolder);
        }

        private static void RemoveFolder(string destinationFolder)
        {
            if (Directory.Exists(destinationFolder))
            {
                Directory.Delete(destinationFolder, true);
            }
        }

        private static bool VerifyFiles(string destinationFolder, string appFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return false;
            }

            var archiveState = Directory
                .GetFiles(
                    appFolder,
                    "gimp-binaries.json"
                )
                .OrderByDescending(x => x)
                .First();

            var state = File.ReadAllText(archiveState);
            var itemInfos = JsonSerializer.Deserialize<List<ItemInfo>>(state);
            foreach (var itemInfo in itemInfos)
            {
                var file = Path.Combine(destinationFolder, itemInfo.File);
                if (!File.Exists(file))
                {
                    return false;
                }

                if ( (new FileInfo(file)).Length  != itemInfo.Size)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Execute(string destinationFolder, string appFolder)
        {
            var appFolderCaseCpecific = new DirectoryInfo(appFolder).Parent.FullName;

            var processes = Process.GetProcessesByName("Launcher");
            foreach (var process in processes)
            {
                try
                {
                    if (process.MainModule.FileName.StartsWith(appFolderCaseCpecific, StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            
            var gimpFolder = Path.Combine(destinationFolder, "bin");
            if (!Directory.Exists(gimpFolder))
            {
                return false;
            }

            var gimp = Directory
                .GetFiles(gimpFolder, "gimp-*.*.exe")
                .OrderBy(x => x.Length)
                .FirstOrDefault();
            if (gimp == null)
                return false;
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(gimp) { WorkingDirectory = gimpFolder });
            return true;
        }
    }

    public class ItemInfo
    {
        public string File { get; set; }
        public long Size { get; set; }
    }
}