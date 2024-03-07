Param([string]$findVersion = "")
$ErrorActionPreference = "Stop"

# Set location to the Solution directory
(Get-Item $PSScriptRoot).Parent.FullName | Push-Location

Import-Module ./msbuild/exechelper.ps1

# Install .NET tooling
exec ./msbuild/dotnet-cli-install.ps1

function Get-ReferenceVersion {
  param (
      [string]$Name
  )
  $Node = $versionFile.SelectSingleNode("Project/PropertyGroup/$Name")
  return $Node.InnerText
}

#just for testing, will remove it later
New-Item -Path ".\artifacts" -Name "packages" -ItemType "directory" -Force
[xml] $versionFile = Get-Content ".\msbuild\DependencyVersions.props"
$newtonsoftVersion = Get-ReferenceVersion "NewtonsoftJsonVersion"
$cmsCoreVersion = Get-ReferenceVersion "CMSCoreVersion"
$cmsUIVersion = Get-ReferenceVersion "CMSUICoreVersion"
$cgVersion = Get-ReferenceVersion "ContentGraphVersion"
$cdVersion = Get-ReferenceVersion "ContentDeliveryVersion"
exec "dotnet" "msbuild msbuild/createzip.proj /t:CreateZipFile /p:findVersion=$findVersion /p:newtonsoftVersion=$newtonsoftVersion /p:cmsCoreVersion=$cmsCoreVersion /p:cmsUIVersion=$cmsUIVersion /p:cgVersion=$cgVersion /p:cdVersion=$cdVersion"

Pop-Location
