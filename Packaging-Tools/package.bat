:: SCRIPT TO BUILD NUGET PACKAGE

:: Build Nuget to local directory
cd ./src/Packaging.Tools
dotnet pack -c Release
pause
