Param([String]$WindowsSDKPath, [String]$SNKPath, [String]$CertPath, [String]$CertPassword)

if(!$WindowsSDKPath) {
    Write-Host "WindowsSDKPath is a required parameter"
    exit 1
}
Write-Host "SDK tools path: " + $WindowsSDKPath

if(!$SNKPath) {
    Write-Host "$SNKPath is a required parameter"
    exit 1
}

if(!$CertPath) {
    Write-Host "$CertPath is a required parameter"
    exit 1
}

$sn = $WindowsSDKPath + "\sn.exe"
if (!(Test-Path $sn)) {
    Write-Error ($sn + " not found")
    exit 1
}

function Get-Cert()
{
    $password = ConvertTo-SecureString $CertPassword -AsPlainText -Force
    return Get-PfxCertificate -FilePath $CertPath -Password $password
}

$rootDir = Get-Location
Write-Host $rootDir

Write-Host "Finding assemblies"
$srcProjects = [Array](Get-ChildItem -Directory -Path (Join-Path ($rootDir) ".\APIs\src\"))

$assemblies = @()
foreach($item in $srcProjects)
{
    $projectAssemblies = (Get-ChildItem -Recurse -Path $item.FullName -File -Filter ($item.Name + ".dll") )
    if($projectAssemblies.length -lt 1){
        Write-Host ("File not found: " + $dllName + " in directory: " + $item.FullName)
        continue
    }

    $assemblies += $projectAssemblies
}

Write-Host "Signing assemblies"
foreach ($assembly in $assemblies)
{
   Write-Host (" Signing " + $assembly.FullName)
   $LASTEXITCODE = 0
   &"$sn" -q -R  $assembly.FullName $SNKPath
   if ($LASTEXITCODE -ne 0)
   {
       exit $LASTEXITCODE
   }
}

$url = "http://timestamp.digicert.com/scripts/timstamp.dll"
$cert = Get-Cert
if ($cert -eq $null)
{
	Write-Error "No certificate has been found, or it is about to expire"
	exit 1
}

foreach($item in $assemblies)
{
    Write-Host ("Authenticode signing " + $item.FullName)
    Set-AuthenticodeSignature -FilePath $item.FullName -Certificate $cert -TimestampServer $url -WarningAction Stop | Out-Null

    $signed = (Get-AuthenticodeSignature -filepath $item.FullName).Status
    if($signed -eq "NotSigned")
    {
        Write-Host ("Authenticode signing failed " + $item.FullName)
        exit 1
    }
}