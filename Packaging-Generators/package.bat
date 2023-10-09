:: SCRIPT TO BUILD NUGET PACKAGE

:: Build Nuget to local directory
dotnet pack src/Packaging.Generators/Packaging.Generators.csproj --output nupkgs -c Release
pause
