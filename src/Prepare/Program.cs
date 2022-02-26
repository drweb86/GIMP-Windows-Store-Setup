using DownloadInstaller;

var setupInfo = await GimpWebSiteUtil.GetDownloadLink();

var dirs = TempDir.GetDirs();

var setupExecutable = await GimpWebSiteUtil.Download(setupInfo.Link, dirs.DownloadSetup, setupInfo.FileName);
InnoSetupHelper.InstallSilently(setupExecutable, dirs.InstallSetup);
InnoSetupHelper.RemoveInstallationSpecificFiles(dirs.InstallSetup);
GimpSetupHelper.CheckExecutableCanBeFound(dirs.InstallSetup, setupInfo.Version);
GimpSetupHelper.Pack(dirs.InstallSetup, dirs.Package);
GimpSetupHelper.CopyLauncherFullTrust(dirs.LauncherFullTrustAnyCpuDebug, dirs.Package);

return 0;