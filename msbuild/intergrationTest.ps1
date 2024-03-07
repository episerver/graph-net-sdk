# NOTE: This script must currently be executed from the solution dir (due to specs)
Param([string] $configuration = "Release", [string] $intergrationTest = "EPiServer.ContentGraph.IntegrationTests", [string] $logger="trx", [string] $verbosity="minimal", [string] $skiptests="false", [string] $testframework="net6.0")
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

# Build dotnet projects
exec "dotnet" "build APIs\src\EpiServer.ContentGraph.sln -c $configuration -v $verbosity"

# Run test projects
if ( $skiptests -ne "true" )
{
  # Integration tests
  exec "dotnet"  "test APIs\src\Testing\$intergrationTest\$intergrationTest.csproj -l $logger -r TestResults -v $verbosity -c $configuration --no-build --no-restore -f $testframework"
}

Pop-Location

