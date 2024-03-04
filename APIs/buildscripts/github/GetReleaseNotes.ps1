<#
    .SYNOPSIS
        Creates release notes based on diff between current and previous release

    .DESCRIPTION
        Creates a release notes.txt based on diff between current and previous release

    .EXAMPLE
        GetReleaseNotes
#>

[CmdletBinding()]
[OutputType([System.Void])]
param(
)

$currentBranch = (git rev-parse --abbrev-ref HEAD)
$latestCommitId = (git rev-parse --short HEAD)
$currentTime = (Get-Date).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")

$releaseNotes = "Release from $currentBranch ($latestCommitId)`n`n"
$releaseNotes += "Created at $currentTime UTC`n`n"

$compareCommit = Invoke-Expression "git describe --tags --abbrev=0"
if (-not $compareCommit) {
    throw "No previous tag found to compare commits from."
}

$gitLogCommand = "git log --no-merges --oneline HEAD --not $compareCommit"
$gitLogs = (Invoke-Expression $gitLogCommand)

$releaseNotes += "# Changes`n`n"

foreach ($logLine in $gitLogs) {
    $commitId = ($logLine -split " ")[0]
    $commitMessage = (git log --format=%B -n 1 $commitId)
    $jiraTasks = Select-String -Pattern "(FIND)-[0-9]{3,10}" -Input $commitMessage -AllMatches
    if ($jiraTasks.Matches) {
        $jiraTasks = $jiraTasks.Matches | ForEach-Object { "[$($_.Value)](https://jira.sso.episerver.net/browse/$($_.Value))" }
    }
    if ($jiraTasks.Count -gt 0) {
        $commitNotes = "- $logLine ($($jiraTasks -join ", "))`n"
    }
    else {
        $commitNotes = "- $logLine`n"
    }

    $releaseNotes += $commitNotes
}

Write-Output $releaseNotes
