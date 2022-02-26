# Submission to Windows Store

This articles describes the workflow of publishing the new version of an application to Windows Store.

In Windows Store terms process of submission of a new version is called **Submission**. Submission includes the packages, information about the new version including version changes, screenshots, etc.

Each new submission starts and is based on previous submission. During new submission its enough to update just package, but there's a possibility to change version descriptions, screenshots, videos and other details.

Texts, pictures and images can be translated to different languages. In this case Windows Store will show user page about GIMP localized to specific language. The same is for package: anything in the package including texts, images and even markup can be altered during localization.

Currently solutions in this folder automotize the packages creation only. However, it is possible to automate the package submissions as well which is a subject of future improvements.

Currently only english language is declared at both store and package.

## Software Requirements

- Windows 11 x-64 bit
- Visual Studio 2022 Community Edition
  - Following components needs to be installed
    - .Net desktop development
    - Universal Windows Platform development
  - ![](img/vs-installer-setup.png)
- 7-Zip x-64 bit
  - 7-zip should be installed to default location.

## Workflow

1. Launch build.cmd
   1. Script will rebuild solution "src\Prepare.sln" using .Net-Core
   2. Then it will execute Prepare project.
      1. Prepare project will navigate to GIMP web-site, download latest setup, silently install it,
      2. After that install dir will be packed and will put it into Launcher project folder.
2. Open "src\App.sln" in **Visual Studio**
3. In **Solution Explorer** right click on "**LauncherPackage**" project, click **Publish** , **Create App Packages** ![](img/create-app-package.png)
4. In **Select Distribution Method** ensure your association is selected and click Next ![](img/select-distribution-method.png)
5. In Select and Configure Packages check that [x] Automatically Increment, [x] x86, [x] x64, [x] Generate artifacts to validate the app with the Windows App Certification Kit and click Next. ![](img/select-and-configure-packages.png)
6. Launch WACK and wait till it will succeed and close it.  ![](img/wack.png)
7. Navigate to src\LauncherPackage\AppPackages and see package prepared for upload with name like LauncherPackage_XXXXX_x86_x64_bundle.appxupload
8. Open Microsoft Partner Center web-site, go to application overview and click Update on last submission to create a new submission from it. It will create a new submission and open it for editing.![](img/update-submission.png)
9. Click Packages ![](img/submission-packages.png)
10. Drop LauncherPackage_XXXXX_x86_x64_bundle.appxupload into Drag and Drop area (1), remove copy of old package (2), click Save (3)  ![](img/submission-packages-update.png)
11. Update information about application (texts, images, new version changes) if necessary at each language ![](img/update-store-listing.png)
12. Click Submit to the Store
13. Witing a day or two for the e-mail with publish approval notification. If approval didn't happen then report with error will be in the e-mail.
