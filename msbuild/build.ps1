# NOTE: This script must currently be executed from the solution dir (due to specs)
Param([string] $configuration = "Release", [string] $logger="trx", [string] $verbosity="minimal", [string] $skiptests="false", [string] $testframework="net6.0")
$ErrorActionPreference = "Stop"

# Enable following line for being able to run the script consecutively in terminal
# Remove-Variable * -ErrorAction SilentlyContinue; Remove-Module *; $error.Clear();

# Set location to the Solution directory
(Get-Item $PSScriptRoot).Parent.FullName | Push-Location

Import-Module .\msbuild\exechelper.ps1

# Install .NET tooling
exec .\msbuild\dotnet-cli-install.ps1

$msbuildPath = (.\msbuild\vswhere.exe -version '[16, 18)' -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe) | Select-Object -First 1
if (!$msbuildPath)
{
    throw "vswhere.exe failed to find msbuild location"
}

#Remove-Item -Path ./zipoutput -Recurse -Force -Confirm:$false -ErrorAction Ignore

# Build dotnet projects
exec "dotnet" "build APIs\src\EPiServer.ContentGraph.sln -c $configuration -v $verbosity"

# Run test projects
if ( $skiptests -ne "true" )
{
  # Unit tests
  exec "dotnet"  "test APIs\src\Testing\EpiServer.ContentGraph.UnitTests\EpiServer.ContentGraph.UnitTests.csproj -l $logger -r TestResults -v $verbosity -c $configuration --no-build --no-restore -f $testframework"

  # Integration tests
  exec "dotnet"  "test APIs\src\Testing\EPiServer.ContentGraph.IntegrationTests\EPiServer.ContentGraph.IntegrationTests.csproj -l $logger -r TestResults -v $verbosity -c $configuration --no-build --no-restore -f $testframework"
}

Pop-Location

