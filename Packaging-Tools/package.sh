#!/bin/bash

# SCRIPT TO BUILD NUGET PACKAGE

# Navigate to project directory
cd ./src/Packaging.Tools

# Build NuGet to local directory
dotnet pack -c Release

# Wait for user input before exiting
read -p "Press [Enter] to continue..."