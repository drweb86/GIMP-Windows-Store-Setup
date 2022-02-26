using System.Text.Json;

namespace DownloadInstaller
{
    static class GimpSetupHelper
    {
        internal static void EnsureExecutableExists(string installDir)
        {
            var binFolder = Path.Combine(installDir, "bin");

            var searchMask = $"gimp-*.*.exe";
            var gimpExecutable = Directory
                .GetFiles(binFolder, searchMask)
                .OrderBy(x => x.Length)
                .FirstOrDefault();

            if (gimpExecutable == null)
            {
                throw new Exception($"Executable cannot be found at {binFolder} by search mask {searchMask}");
            }

            Console.WriteLine($"Launcher wrapper will execute this: {gimpExecutable}");
        }

        internal static void CopyLauncherFullTrust(string launcherFullTrustOutputDir, string launcherPackageProjectDir)
        {
            var destinationDir = Path.Combine(launcherPackageProjectDir, "Win32");
            Console.WriteLine($"Copy {launcherFullTrustOutputDir} to {destinationDir}");
            CopyFilesRecursively(launcherFullTrustOutputDir, destinationDir);
            Console.WriteLine($"Copy completed.");
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        internal static void CreateBinariesArchiveAndState(string installDir, string launcherProjectDir)
        {
            var win32Dir = Path.Combine(launcherProjectDir, "Win32");
            CreateBinariesArchive(installDir, win32Dir);
            CreateBinariesState(installDir, win32Dir);
        }

        private static void CreateBinariesArchive(string installDir, string win32Dir)
        {
            var archive = Path.Combine(win32Dir, $"gimp-binaries.7z");

            if (File.Exists(archive))
                File.Delete(archive);

            Console.WriteLine($"Packing {installDir} to {archive}...");

            ProcessHelper.Execute(@"c:\Program Files\7-zip\7z.exe", $"a -t7z -mx=9 -mfb=273 -ms -md=31 -myx=9 -mtm=- -mmt -mmtf -md=1536m -mmf=bt3 -mmc=10000 -mpb=0 -mlc=0 gimp-binaries.7z \"{installDir}\"\\*", win32Dir,
                out string stdOutput, out string stdError, out int returnCode);

            if (returnCode != 0)
            {
                Console.WriteLine(stdOutput);
                Console.Error.WriteLine(stdError);
                throw new Exception($"Failed to pack {installDir} to {archive}");
            }
            Console.WriteLine($"Packed");
        }

        internal static void CreateBinariesState(string installSetup, string destinationDir)
        {
            Console.WriteLine($"Saving state");
            var jsonFile = Path.Combine(destinationDir, $"gimp-binaries.json");

            var data = new List<ItemInfo>();
            var files = Directory.GetFiles(installSetup, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                data.Add(new ItemInfo() { File = fileInfo.FullName.Substring(installSetup.Length + 1), Size = fileInfo.Length });
            }
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(jsonFile, json);
            Console.WriteLine($"State saved");
        }
    }
}
