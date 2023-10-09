:: SCRIPT TO INSTALL BUILT NUGET PACKAGE AS GLOBAL TOOL

:: Install the package as global tool
cd ./src/Packaging.Tools

dotnet tool install --global --add-source ./nupkg Packaging.Tools
pause
