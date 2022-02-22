using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
