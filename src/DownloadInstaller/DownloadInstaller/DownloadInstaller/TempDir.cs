using System.Reflection;

namespace DownloadInstaller
{
    class Dirs
    {
        public string Temp { get; set; }
        public string InstallSetup { get; set; }
        public string DownloadSetup { get; set; }
    }

    internal static class TempDir
    {
        public static Dirs GetDirs()
        {
            Log.Info("Getting temporary folder");
            var tempFolder = new DirectoryInfo(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "../../../../../..",
                    "temp")).FullName;

            var dirs = new Dirs();
            dirs.Temp = tempFolder;
            dirs.DownloadSetup = Path.Combine(dirs.Temp, "download-setup");
            dirs.InstallSetup = Path.Combine(dirs.Temp, "install-setup");

            if (Directory.Exists(dirs.InstallSetup))
            {
                Log.Info("Cleaning the install folder");
                Directory.Delete(dirs.InstallSetup, true);
            }
            Directory.CreateDirectory(dirs.Temp);
            Directory.CreateDirectory(dirs.DownloadSetup);
            Directory.CreateDirectory(dirs.InstallSetup);

            return dirs;
        }
    }
}
