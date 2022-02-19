using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInstaller
{
    internal static class GimpSetupHelper
    {
        internal static void CheckExecutableCanBeFound(string installSetup)
        {
            Log.Info("Checking that GIMP executable present in installation place");

            var binFolder = Path.Combine(installSetup, "bin");

            var searchMask = "gimp-*.exe";
            var gimpExecutable = Directory.GetFiles(binFolder, searchMask)
                .OrderBy(f => f.Length)
                .FirstOrDefault();
            
            if (gimpExecutable == null)
            {
                Log.Error($"GIMP executable cannot be found at {binFolder} by search mask {searchMask}");
                throw new Exception("Executable cannot be found.");
            }

            Log.Confirm($"Gimp executable: {gimpExecutable}");

        }
    }
}
