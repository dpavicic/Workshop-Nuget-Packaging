#!/bin/bash

# SCRIPT TO INSTALL BUILT NUGET PACKAGE AS GLOBAL TOOL

# Navigate to project directory
cd ./src/Packaging.Tools

# Install the package as a global tool
dotnet tool install --global --add-source ./nupkg Packaging.Tools

# Wait for user input before exiting
read -p "Press [Enter] to continue..."