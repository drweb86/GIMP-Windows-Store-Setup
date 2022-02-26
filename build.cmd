@echo off

echo Building Prepare.sln with Release configuration

dotnet build "src/Prepare.sln" --configuration "Release"

if %ERRORLEVEL% neq 0 (
	echo Failed to build Prepare.sln with Release configuration
	goto ERROR_EXIT
)

"src\Prepare\bin\Debug\net6.0\Prepare.exe"
if %ERRORLEVEL% neq 0 (
	echo Prepare.exe has failed!
	goto ERROR_EXIT
)

echo Success!
exit 0

:ERROR_EXIT
echo Failed.
exit 1
