#!/bin/bash

# SCRIPT TO BUILD NUGET PACKAGE

# Build NuGet to local directory
dotnet pack src/Packaging.Generators/Packaging.Generators.csproj --output nupkgs -c Release

# Wait for user input before exiting
read -p "Press [Enter] to continue..."