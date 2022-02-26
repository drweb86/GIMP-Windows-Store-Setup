using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DownloadInstaller
{
    static class GimpSetupHelper
    {
        internal static void CheckExecutableCanBeFound(string installSetup)
        {
            var binFolder = Path.Combine(installSetup, "bin");

            var searchMask = $"gimp-*.*.exe";
            var gimpExecutable = Directory
                .GetFiles(binFolder, searchMask)
                .OrderBy(x => x.Length)
                .FirstOrDefault();

            if (gimpExecutable == null)
            {
                throw new Exception($"GIMP executable cannot be found at {binFolder} by search mask {searchMask}");
            }

            Console.WriteLine($"Gimp executable was detected: {gimpExecutable}");
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

        internal static void Pack(string installSetup, string package)
        {
            var destinationDir = Path.Combine(package, "Win32");

            var archive = Path.Combine(destinationDir, $"gimp-binaries.7z");
            if (File.Exists(archive))
                File.Delete(archive);
            Log.Debug($"Packing {installSetup} to {archive}.\nPlease wait.");
            ProcessHelper.Execute(@"c:\Program Files\7-zip\7z.exe", $"a -t7z -mx=9 -mfb=273 -ms -md=31 -myx=9 -mtm=- -mmt -mmtf -md=1536m -mmf=bt3 -mmc=10000 -mpb=0 -mlc=0 gimp-binaries.7z \"{installSetup}\"\\*", destinationDir,
                out string stdOutput, out string stdError, out int returnCode);
            if (returnCode != 0)
            {
                Log.Error(stdOutput);
                Log.Error(stdError);
                throw new Exception();
            }
            Log.Debug($"Packed");
            Log.Debug($"Saving state");
            WriteFilesList(installSetup, destinationDir);
            Log.Debug($"State saved");
        }

        internal static void WriteFilesList(string installSetup, string destinationDir)
        {
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
        }
    }
}
