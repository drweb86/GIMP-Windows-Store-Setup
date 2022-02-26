namespace DownloadInstaller
{
    internal class InnoSetupHelper
    {
        public static void Install(string setup, string installDir)
        {
            Console.WriteLine($"Installing {setup} to {installDir}...");
            ProcessHelper.Execute(setup, $"/VERYSILENT /SUPRESSMSGBOXES /NOCANCEL /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /NORESTARTAPPLICATIONS /DIR=\"{installDir}\" /CURRENTUSER", Path.GetDirectoryName(setup), out string stdOutput, out string stdError, out int exitCode);
            Console.WriteLine(stdOutput);
            if (!string.IsNullOrWhiteSpace(stdError))
            {
                Console.Error.WriteLine(stdError);
            }

            if (exitCode != 0)
            {
                throw new Exception($"Installation has failed with exit code {exitCode}");
            }
        }

        public static void DeleteSetupFiles(string installDir)
        {
            Console.WriteLine($"Clearning up Install specific files at {installDir}");
            var uninstFolder = Path.Combine(installDir, "uninst");
            if (Directory.Exists(uninstFolder))
                Directory.Delete(uninstFolder, true);
        }
    }
}
