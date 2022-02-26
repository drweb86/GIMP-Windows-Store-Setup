using DownloadInstaller;

var windowsSetupUrl = GimpWebSiteUtil.GetWindowsSetupUrl();

var applicationFolders = ApplicationFoldersProvider.Provide();

var setupExecutable = await GimpWebSiteUtil.Download(windowsSetupUrl, applicationFolders.DownloadSetup);
InnoSetupHelper.Install(setupExecutable, applicationFolders.InstallDir);
InnoSetupHelper.DeleteSetupFiles(applicationFolders.InstallDir);
GimpSetupHelper.EnsureExecutableExists(applicationFolders.InstallDir);
GimpSetupHelper.CreateBinariesArchiveAndState(applicationFolders.InstallDir, applicationFolders.LauncherProjectDir);
GimpSetupHelper.CopyLauncherFullTrust(applicationFolders.LauncherFullTrustAnyCpuDebug, applicationFolders.LauncherProjectDir);

return 0;