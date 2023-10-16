#!/bin/bash

# SCRIPT TO BUILD NUGET PACKAGE

# Navigate to project directory
cd ./My.Awesome.Library

# Build, Test and Package NuGet to local directory
dotnet build --configuration Release
dotnet test --configuration Release
dotnet pack --output nupkgs -c Release

# Wait for user input before exiting
read -p "Press [Enter] to continue..."