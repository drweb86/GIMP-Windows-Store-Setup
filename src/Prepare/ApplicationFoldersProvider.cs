using System.Reflection;

namespace DownloadInstaller
{
    internal static class ApplicationFoldersProvider
    {
        public static ApplicationFolders Provide()
        {
            var tempFolder = new DirectoryInfo(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "../../../..",
                    "temp")).FullName;

            var dirs = new ApplicationFolders();
            dirs.Temp = tempFolder;
            dirs.DownloadSetup = Path.Combine(dirs.Temp, "download-setup");
            dirs.InstallDir = Path.Combine(dirs.Temp, "install-setup");
            dirs.LauncherProjectDir = Path.Combine(dirs.Temp, "../LauncherPackage");
            dirs.LauncherFullTrustAnyCpuDebug = Path.Combine(dirs.Temp, "..", "Launcher.FullTrust", "bin", "Release", "net6.0-windows");
            dirs.ArchiveFolder = Path.Combine(dirs.Temp, "Archive");

            if (Directory.Exists(dirs.InstallDir))
            {
                Console.WriteLine($"Cleaning folder {dirs.InstallDir}");
                Directory.Delete(dirs.InstallDir, true);
            }

            if (Directory.Exists(dirs.ArchiveFolder))
            {
                Console.WriteLine($"Cleaning folder {dirs.ArchiveFolder}");
                Directory.Delete(dirs.ArchiveFolder, true);
            }

            Directory.CreateDirectory(dirs.Temp);
            Directory.CreateDirectory(dirs.DownloadSetup);
            Directory.CreateDirectory(dirs.InstallDir);
            Directory.CreateDirectory(dirs.ArchiveFolder);

            return dirs;
        }
    }
}
