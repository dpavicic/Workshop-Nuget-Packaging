#!/bin/bash

# SCRIPT TO BUILD NUGET PACKAGE

# Build all libraries in RELEASE mode
dotnet build src/My.LibraryA/My.LibraryA.csproj --configuration Release
dotnet build src/My.LibraryB/My.LibraryB.csproj --configuration Release

# Package Nuget to local directory using nuget.exe and .nuspec manifest
nuget pack My.Package.Bundled.nuspec -OutputDirectory nupkgs

# Wait for user input before exiting
read -p "Press [Enter] to continue..."