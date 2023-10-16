:: SCRIPT TO BUILD NUGET PACKAGE

:: Build Nuget to local directory
cd ./My.Awesome.Library
dotnet build --configuration Release
dotnet test --configuration Release
dotnet pack --output nupkgs -c Release
pause
