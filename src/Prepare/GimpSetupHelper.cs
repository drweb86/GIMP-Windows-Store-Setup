using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DownloadInstaller
{
    internal static class GimpSetupHelper
    {
        internal static void CheckExecutableCanBeFound(string installSetup, string version)
        {
            Log.Debug("Checking that GIMP executable present in installation place");

            var binFolder = Path.Combine(installSetup, "bin");

            var parsedVersion = new Version(version);

            var searchMask = $"gimp-{parsedVersion.Major}.{parsedVersion.Minor}.exe";
            var gimpExecutable = Directory.GetFiles(binFolder, searchMask)
                .SingleOrDefault();
            
            if (gimpExecutable == null)
            {
                Log.Error($"GIMP executable cannot be found at {binFolder} by search mask {searchMask}");
                throw new Exception("Executable cannot be found.");
            }

            Log.Debug($"Gimp executable: {gimpExecutable}");

        }

        internal static void CopyLauncherFullTrust(string launcherFullTrustAnyCpuDebug, string package)
        {
            var destinationDir = Path.Combine(package, "Win32");
            Log.Debug($"Copy launcher full trust executable from {launcherFullTrustAnyCpuDebug} to {destinationDir}");
            CopyFilesRecursively(launcherFullTrustAnyCpuDebug, destinationDir);
            Log.Debug($"Copy completed.");
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

        internal static void Pack(string installSetup, string archiveFolder, string version)
        {
            var archive = Path.Combine(archiveFolder, $"gimp-binaries-{version}.zip");
            Log.Debug($"Packing {installSetup} to {archive}.\nPlease wait.");
            ZipFile.CreateFromDirectory(installSetup, archive, CompressionLevel.Optimal, false);
            Log.Debug($"Packed");
            Log.Debug($"Saving state");
            WriteFilesList(installSetup, archiveFolder, version);
            Log.Debug($"State saved");
        }

        internal static void WriteFilesList(string installSetup, string archiveFolder, string version)
        {
            var jsonFile = Path.Combine(archiveFolder, $"gimp-binaries-{version}.json");

            var data = new List<ItemInfo>();
            var files = Directory.GetFiles(installSetup, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                data.Add(new ItemInfo() { File = fileInfo.FullName.Substring(installSetup.Length + 1), Size = fileInfo.Length });
            }
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(jsonFile, json);
        }
    }
}
