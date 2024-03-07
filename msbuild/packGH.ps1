Param([string]$version = "", [string] $configuration = "Release")
$ErrorActionPreference = "Stop"

# Set location to the Solution directory
(Get-Item $PSScriptRoot).Parent.FullName | Push-Location

Import-Module .\msbuild\exechelper.ps1

# Install .NET tooling
exec .\msbuild\dotnet-cli-install.ps1

# Packaging public packages
exec "dotnet" "pack --no-restore --no-build -c $configuration /p:PackageVersion=$version APIs\src\EPiServer.ContentGraph.sln"
