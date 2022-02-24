# GIMP Windows Store Setup

The project for setup and GIMP Windows Store Setup maintanance.

# Requirements

Windows 11 x-64
Visual Studio 2022
7-Zip X64 installed to default location

# Workflow

1. Open "src\Prepare.sln" in Visual Studio and Menu \ Build \ Rebuild Solution
2. Solution Explorer right click "Prepare" project, "Set as startup project"
3. Launch it (F5)

4. Open "src\Wrapper.sln" in Visual Studio and Menu \ Build \ Rebuild Solution
5. Solution Explorer right click "LauncherPackage" project, Publish \ Create App Packages

6. Publish src\LauncherPackage\AppPackages\LauncherPackage_XXXX_bundle.appxupload to store

