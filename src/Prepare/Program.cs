using DownloadInstaller;

Log.Info("Application performs preparation steps for packaging the GIMP");

var setupInfo = await GimpWebSiteUtil.GetDownloadLink();

var dirs = TempDir.GetDirs();

var setupExecutable = await GimpWebSiteUtil.Download(setupInfo.Link, dirs.DownloadSetup, setupInfo.FileName);
InnoSetupHelper.InstallSilently(setupExecutable, dirs.InstallSetup);
InnoSetupHelper.RemoveInstallationSpecificFiles(dirs.InstallSetup);
GimpSetupHelper.CheckExecutableCanBeFound(dirs.InstallSetup);
ManifestHelper.UpdateVersion(dirs, setupInfo.Version);
Log.Info("Preparation is completed. Procceed with packaging.");

return 0;