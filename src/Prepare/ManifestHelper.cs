using System.Xml;

namespace DownloadInstaller
{
    internal class ManifestHelper
    {
        public static void UpdateVersion(Dirs dirs, string version)
        {
            var file = Path.Combine(dirs.Package, "Package.appxmanifest");
            Log.Info($"Update version {version} in {file}.");

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(file);

            bool found = false;
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes) // XPATH does nt work
            {
                if (node.Name == "Identity")
                {
                    var parsedVersion = new Version(version);
                    var parsedVersionWithRevision = new Version(parsedVersion.Major, parsedVersion.Minor, parsedVersion.Build, 0);
                    node.Attributes["Version"].Value = parsedVersionWithRevision.ToString(4);
                    found = true;
                }
            }

            if (!found)
            {
                Log.Error($"Node with version was not found!");
                throw new Exception("Node with version was not found.");
            }

            if (File.Exists(file))
                File.Delete(file);
            xmlDoc.Save(file);
            Log.Info($"Version is updated.");
        }
    }
}
