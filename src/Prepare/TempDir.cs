using System.Reflection;

namespace DownloadInstaller
{
    internal static class TempDir
    {
        public static Dirs GetDirs()
        {
            Log.Debug("Getting temporary folder");
            var tempFolder = new DirectoryInfo(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "../../../..",
                    "temp")).FullName;

            var dirs = new Dirs();
            dirs.Temp = tempFolder;
            dirs.DownloadSetup = Path.Combine(dirs.Temp, "download-setup");
            dirs.InstallSetup = Path.Combine(dirs.Temp, "install-setup");
            dirs.Package = Path.Combine(dirs.Temp, "../LauncherPackage");
            dirs.LauncherFullTrustAnyCpuDebug = Path.Combine(dirs.Temp, "..", "Launcher.FullTrust", "bin", "Release", "net6.0-windows");
            dirs.ArchiveFolder = Path.Combine(dirs.Temp, "Archive");

            if (Directory.Exists(dirs.InstallSetup))
            {
                Log.Debug("Cleaning the install folder");
                Directory.Delete(dirs.InstallSetup, true);
            }
            if (Directory.Exists(dirs.ArchiveFolder))
            {
                Log.Debug("Cleaning the archive folder");
                Directory.Delete(dirs.ArchiveFolder, true);
            }
            Directory.CreateDirectory(dirs.Temp);
            Directory.CreateDirectory(dirs.DownloadSetup);
            Directory.CreateDirectory(dirs.InstallSetup);
            Directory.CreateDirectory(dirs.ArchiveFolder);

            return dirs;
        }
    }
}
