Param([string]$branchName, [Int]$buildCounter, [String]$publishPackages)

if (!$branchName -or !$buildCounter) {
    Write-Error "`$branchName and `$buildCounter parameters must be supplied"
    exit 1
}

Function GetVersion($path) {
    [xml] $versionFile = Get-Content $path
    return $versionFile.SelectSingleNode("Project/PropertyGroup/VersionPrefix").InnerText
}

$assemblyVersion = GetVersion "msbuild/version.props"
 
if (!$assemblyVersion) {
    $assemblyVersion = "14.0.0"
}

switch -wildcard ($branchName) {
    "master" { 
        $preReleaseInfo = ""
        $publishPackages = "True"
    }
    "develop" { 
        $preReleaseInfo = "-inte-{0:D6}"
        $publishPackages = "True"
    }
    "bugfix/*" { 
        $preReleaseInfo = "-ci-{0:D6}"
        $publishPackages = "True"
    }
    "hotfix/*" { 
        $preReleaseInfo = ""
        $publishPackages = "True"
    }
    "release/*" { 
        $preReleaseInfo = "-pre-{0:D6}"
        $publishPackages = "True"
    }
    "feature/*" { 
        $isMatch = $branchName -match ".*/([A-Z]+-[\d]+)-"
        if ($isMatch -eq $TRUE) {
            $feature = $Matches[1]
            $preReleaseInfo = "-feature-$feature-{0:D6}"
            $publishPackages = "True"
        }
        else {
            $preReleaseInfo = "-feature-{0:D6}" 
        }
    }
    default { $preReleaseInfo = "-ci-{0:D6}" } 
}

$informationalVersion = "$assemblyVersion$preReleaseInfo" -f $buildCounter
 
"AssemblyVersion: $assemblyVersion"
"AssemblyInformationalVersion: $informationalVersion"

"##teamcity[setParameter name='packageVersion' value='$informationalVersion']"
"##teamcity[setParameter name='publishPackages' value='$publishPackages']"

#Set DNX_BUILD_VERSION for dnx compiling (without dash)
$dnxPreReleaseInfo = ""
if ($preReleaseInfo.Length -gt 0) {
    $dnxPreReleaseInfo = ($preReleaseInfo.Substring(1)) -f $buildCounter
}
"##teamcity[setParameter name='buildSuffix' value='$dnxPreReleaseInfo']"
"##teamcity[setParameter name='env.DNX_BUILD_VERSION' value='$dnxPreReleaseInfo']"