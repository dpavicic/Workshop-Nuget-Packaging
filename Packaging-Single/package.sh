#!/bin/bash

# SCRIPT TO BUILD NUGET PACKAGE

# Navigate to project directory
cd ./My.Awesome.Library

# Build NuGet to local directory
dotnet pack --output nupkgs -c Release

# Wait for user input before exiting
read -p "Press [Enter] to continue..."