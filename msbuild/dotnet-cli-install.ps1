Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
#& ./msbuild/dotnet-install.ps1 --install-dir "C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App" -Architecture x64 -AzureFeed "http://dotnetcli.azureedge.net/dotnet" -JsonFile "./global.json"
& ./msbuild/dotnet-install.ps1 -Architecture x64 -AzureFeed "https://dotnetcli.azureedge.net/dotnet" -JsonFile "./global.json"
if($LASTEXITCODE -ne 0) { throw "Failed" }