@echo off

echo Building Prepare.sln with Release configuration

dotnet build "src/Prepare.sln" --configuration "Release"

if errorlevel 1 (
	echo Failed to build Prepare.sln with Release configuration
	goto ERROR_EXIT
)

echo Success!
exit 0

:ERROR_EXIT
echo Failed.
exit 1
