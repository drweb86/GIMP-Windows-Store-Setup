using System.Xml;

namespace DownloadInstaller
{
    internal class ManifestHelper
    {
        public static void UpdateVersion(Dirs dirs, string version)
        {
            var file = Path.Combine(dirs.Package, "Package.appxmanifest");
            Log.Debug($"Update version & executable name {version} in {file}.");

            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(file);

            var parsedVersion = new Version(version);
            var parsedVersionWithRevision = new Version(parsedVersion.Major, parsedVersion.Minor, parsedVersion.Build, 0);
            xmlDoc.DocumentElement.Go("Identity").SetAttr("Version", parsedVersionWithRevision.ToString(4));
            xmlDoc.DocumentElement.Go("Applications").Go("Application").Go("Extensions").Go("desktop:Extension").SetAttr("Executable", $"app\\bin\\gimp-{parsedVersion.Major}.{parsedVersion.Minor}.exe");

            if (File.Exists(file))
                File.Delete(file);
            xmlDoc.Save(file);
            Log.Debug($"Version is updated.");
        }
    }

}
