#!/bin/bash

# SCRIPT TO UNINSTALL BUILT NUGET PACKAGE AS GLOBAL TOOL

# Uninstall the package as a global tool
# Provided name should be package ID, NOT the command name
dotnet tool uninstall -g Packaging.Tools

# Wait for user input before exiting
read -p "Press [Enter] to continue..."