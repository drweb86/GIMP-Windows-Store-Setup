using DownloadInstaller;

var setupInfo = GimpWebSiteUtil.GetWindowsSetupDownloadLinkInfo();

var dirs = ApplicationFoldersProvider.Provide();

var setupExecutable = await GimpWebSiteUtil.Download(setupInfo.Link, dirs.DownloadSetup, setupInfo.FileName);
InnoSetupHelper.InstallSilently(setupExecutable, dirs.InstallSetup);
InnoSetupHelper.RemoveInstallationSpecificFiles(dirs.InstallSetup);
GimpSetupHelper.CheckExecutableCanBeFound(dirs.InstallSetup);
GimpSetupHelper.Pack(dirs.InstallSetup, dirs.Package);
GimpSetupHelper.CopyLauncherFullTrust(dirs.LauncherFullTrustAnyCpuDebug, dirs.Package);

return 0;