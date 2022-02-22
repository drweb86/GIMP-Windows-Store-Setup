using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInstaller
{
    internal class InnoSetupHelper
    {
        public static void InstallSilently(string setup, string destinationFolder)
        {
            Log.Debug($"Installing silently {setup} to {destinationFolder}.\nPlease wait.");
            ProcessHelper.Execute(setup, $"/VERYSILENT /SUPRESSMSGBOXES /NOCANCEL /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /NORESTARTAPPLICATIONS /DIR=\"{destinationFolder}\" /CURRENTUSER", Path.GetDirectoryName(setup), out string stdOutput, out string stdError, out int exitCode);
            Log.Debug(stdOutput);
            if (!string.IsNullOrWhiteSpace(stdError))
            {
                Log.Error(stdOutput);
            }
            if (exitCode != 0)
            {
                Log.Error($"Installation has failed with exit code {exitCode}");
                throw new Exception("Installation has failed");
            }
        }

        public static void RemoveInstallationSpecificFiles(string destinationFolder)
        {
            Log.Debug($"Clearning up Install specific files at {destinationFolder}");
            var uninstFolder = Path.Combine(destinationFolder, "uninst");
            if (Directory.Exists(uninstFolder))
                Directory.Delete(uninstFolder, true);
        }
    }
}
