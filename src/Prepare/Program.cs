using DownloadInstaller;

Log.Info("Preparing...");

var setupInfo = await GimpWebSiteUtil.GetDownloadLink();

var dirs = TempDir.GetDirs();

var setupExecutable = await GimpWebSiteUtil.Download(setupInfo.Link, dirs.DownloadSetup, setupInfo.FileName);
InnoSetupHelper.InstallSilently(setupExecutable, dirs.InstallSetup);
InnoSetupHelper.RemoveInstallationSpecificFiles(dirs.InstallSetup);
GimpSetupHelper.CheckExecutableCanBeFound(dirs.InstallSetup, setupInfo.Version);
GimpSetupHelper.Pack(dirs.InstallSetup, dirs.ArchiveFolder, setupInfo.Version);
ManifestHelper.UpdateVersion(dirs, setupInfo.Version);
Log.Info("Completed. Now open App.sln and procceed with packaging.");

return 0;