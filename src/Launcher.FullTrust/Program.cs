using SevenZipExtractor;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Launcher.FullTrust
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var applicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var binariesArchive = Path.Combine(applicationFolder, "gimp-binaries.7z");
            var binariesState = Path.Combine(binariesArchive, "gimp-binaries.json");

            var destinationFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(binariesArchive));

            if (!IsFilesValid(destinationFolder, binariesState))
            {
                RemoveFolder(destinationFolder);
                Directory.CreateDirectory(destinationFolder);
                using (var archiveFile = new ArchiveFile(binariesArchive))
                {
                    archiveFile.Extract(destinationFolder);
                }
            }

            KillParentUWA(applicationFolder);
            LaunchExecutable(destinationFolder);
        }

        private static void RemoveFolder(string destinationFolder)
        {
            if (Directory.Exists(destinationFolder))
            {
                Directory.Delete(destinationFolder, true);
            }
        }

        private static bool IsFilesValid(string destinationFolder, string binariesState)
        {
            if (!Directory.Exists(destinationFolder))
            {
                return false;
            }

            var binariesStateContent = File.ReadAllText(binariesState);
            var itemInfos = JsonSerializer.Deserialize<List<ItemInfo>>(binariesStateContent);
            foreach (var itemInfo in itemInfos)
            {
                var file = Path.Combine(destinationFolder, itemInfo.File);
                if (!File.Exists(file))
                {
                    return false;
                }

                if (new FileInfo(file).Length != itemInfo.Size)
                {
                    return false;
                }
            }

            return true;
        }

        private static void LaunchExecutable(string destinationFolder)
        {
            var binFolder = Path.Combine(destinationFolder, "bin");

            var gimp = Directory
                .GetFiles(binFolder, "gimp-*.*.exe")
                .OrderBy(x => x.Length)
                .FirstOrDefault();

            if (gimp == null)
                return;

            Process.Start(new ProcessStartInfo(gimp) { WorkingDirectory = binFolder });
        }

        private static void KillParentUWA(string appFolder)
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
        }
    }
}