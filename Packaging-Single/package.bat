:: SCRIPT TO BUILD NUGET PACKAGE

:: Build Nuget to local directory
cd ./My.Awesome.Library
dotnet pack --output nupkgs -c Release
pause
