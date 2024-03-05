<#
    .SYNOPSIS
        Create release in Jira and tag issues for the release.

    .DESCRIPTION
        Analyze commits in current branch to see which are part of the release. Create release in Jira and tag issues in the commits for the release.

    .PARAMETER Username
        Username for Jira user

    .PARAMETER Password
        Password for Jira user

    .PARAMETER JiraUrl
        URL for Jira API

    .PARAMETER PreVersion
        Pre-release number to append to release name, for example a build number.

    .PARAMETER Released
        Mark current release as released in Jira and tag, when this is specified no new issues will be added to the release.

    .PARAMETER DefaultPackage
        Name of package, for example "Episerver.PaaS"

    .PARAMETER JiraProject
        Name of project to create release in

    .PARAMETER StoryWithSubtaskIssue
        Story with sub-tasks that should be tagged in release, normally sub-tasks aren't tagged.

    .EXAMPLE
        .\SetJiraRelease.ps1 -Username "myusername" -Password "mypassword" -PreVersion "1234"
        Create release with PreVersion

    .EXAMPLE
        .\SetJiraRelease.ps1 -Username "myusername" -Password "mypassword" -Released
        Mark the current release as released.
#>

[CmdletBinding(SupportsShouldProcess=$true)]
[OutputType([System.Void])]
param(
    [Parameter(Mandatory = $true)]
    [string] $Username,

    [Parameter(Mandatory = $true)]
    [string] $Password,

    [Parameter(Mandatory = $false)]
    [string] $JiraUrl = "https://jira.sso.episerver.net",

    [Parameter(Mandatory = $false)]
    [string] $PreVersion,

    [Parameter(Mandatory = $false)]
    [switch] $Released,

    [Parameter(Mandatory = $false)]
    [string] $DefaultPackage = "Optimizely.Graph.Client",

    [Parameter(Mandatory = $false)]
    [string] $JiraProject = "FIND",

    [Parameter(Mandatory = $false)]
    [string[]] $StoryWithSubtaskIssue = ""
)

Import-Module "$PSScriptRoot\JiraPS" -Force

# # NOTE: PaaS automation specific to get current version from repository
# Import-Module "$PSScriptRoot\EpiAutomationDev\EpiAutomationDev.psd1" -Verbose:$false -ErrorAction Stop -Force
# $packageVersionFilePath = (Get-DevRepoInfo).PackageVersionFile
Function GetVersion($path) {
    [xml] $versionFile = Get-Content $path
    return $versionFile.SelectSingleNode("Project/PropertyGroup/VersionPrefix").InnerText
}

$releaseVersion = GetVersion "msbuild/version.props"

# $releaseVersion = Get-Content $releaseVersionFilePath
if (-not $releaseVersion) {
    throw "Package version not found."
}

$releaseName = $packageName = "$DefaultPackage $releaseVersion"
# if ($PreVersion) {
#     $packageName = "$DefaultPackage $PreVersion"
# }

Write-Output "Release name: $releaseName"
Write-Output "Package name: $packageName"

Set-JiraConfigServer -Server $JiraUrl
$securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force
$credential = [PSCredential]::new($Username, $securePassword)

# List commits for new release
$compareCommit = Invoke-Expression "git describe --tags --abbrev=0"
if (-not $compareCommit) {
    throw "No previous tag found to compare commits from."
}
$gitLogs = Invoke-Expression "git log HEAD ^$compareCommit"
$jiraTasks = @(Select-String -Pattern "((?<!([A-Z]{1,10})-?)[A-Z]+-\d+)" -Input $gitLogs -AllMatches | ForEach-Object { $_.Matches.Value } | Sort-Object -Unique)

$splitGitLogs = [Regex]::Matches(($gitLogs -join "`n"), "^commit [0-9a-f]+`$((?:(?!^commit)[\w\W])+)", [System.Text.RegularExpressions.RegexOptions]::Multiline)
foreach ($splitGitLog in $splitGitLogs) {
    $jiraTasksInCommit = ($jiraTasks | Where-Object { $splitGitLog.Value.Contains($_) }).Count
    if ($jiraTasksInCommit -eq 0) {
        Write-Warning "No Jira task found:`n$splitGitLog"
    }
}

# List commits from previous versions that might not be tagged yet if the story wasn't ready
$gitPreviousVersionListCommand = "git for-each-ref refs/tags/ --count=3 --sort=-version:refname --format=""%(refname:short)"""
$gitPreviousVersions = (Invoke-Expression $gitPreviousVersionListCommand)

$previousVersionTasks = @()
foreach ($version in $gitPreviousVersions) {
    $previousTag = Invoke-Expression "git describe --tags --abbrev=0 $version^"
    if (-not $previousTag) {
        Write-Warning "Previous tag not found for $version"
        continue
    }

    $versionLogs = Invoke-Expression "git log $version ^$previousTag"
    $previousVersionTasks += @(Select-String -Pattern "((?<!([A-Z]{1,10})-?)[A-Z]+-\d+)" -Input $versionLogs -AllMatches | ForEach-Object { $_.Matches.Value } | Sort-Object -Unique)
}

$previousVersionTasks = $previousVersionTasks | Sort-Object -Unique | Where-Object { $_ -notin $jiraTasks }

# Only get some issue-fields to improve performance
$jiraIssueFilter = "key", "issuetype", "status", "summary", "fixVersions", "parent"

# Issue type name and ID in Jira
# Bug            1
# Task           3
# Sub-task       5
# Bug Sub-task   10101
# Story          10001
# Tech Story     10003
# Support Case   10300

$releaseTaskIssueTypes = @(1, 3, 5)
$releaseTaskIssueStatuses = @("Resolved", "Testing", "Closed")

$releaseStoryIssueTypes = @(10101, 10001, 10003, 10300)
$releaseStoryIssueStatuses = @("Closed")

$issuesWithNugetPackageVersion = @(1, 3, 5, 10001, 10003, 10101, 10300)

$issuesInRelease = @()

# Only tag new issues in release if Released-switch isn't specified
if (-not $Released.IsPresent) {
    # Look for issues in Jira to see if they should be included in release depending on issue type
    Write-Verbose "Searching for tasks to be released."
    foreach ($jiraTask in $jiraTasks) {
        try {
            $issue = Get-JiraIssue -Key $jiraTask -Credential $credential -Fields $jiraIssueFilter
        }
        catch {
            if ($_.Exception.Message -like "Issue Does Not Exist") {
                $issue = $null
            }
            else {
                throw "Error while retrieving $($jiraTask): $($_.Exception.Message)`n$($_.ScriptStackTrace)"
            }
        }

        if (-not $issue) {
            Write-Warning "Issue $jiraTask not found"

            foreach ($missingJiraLog in ($splitGitLogs | Where-Object { $_.Value.Contains($jiraTask) })) {
                Write-Warning $missingJiraLog
            }

            continue
        }

        $issueDescription = "$($issue.Key) $($issue.issuetype.name) $($issue.Summary)"

        if ($issue.fixVersions | Where-Object { $_.name -like "$DefaultPackage *" -and $_.released -eq $true }) {
            # Ignore stories that are already released
            if ($issue.issuetype.id -in $releaseStoryIssueTypes) {
                Write-Output "- Already released: $issueDescription"
                continue
            }
        }

        if ($issue.issuetype.id -in $releaseTaskIssueTypes -and $issue.Status -in $releaseTaskIssueStatuses) {
            # Bug commit
            Write-Output "+ $issueDescription"
            $issuesInRelease += $issue
        }
        elseif ($issue.issuetype.id -in $releaseStoryIssueTypes -and $issue.Status -in $releaseStoryIssueStatuses) {
            # Story commit
            Write-Output "+ $issueDescription"
            $issuesInRelease += $issue
        }
        elseif ($issue.parent.key -in $StoryWithSubtaskIssue) {
            # Continuous improvement commit
            Write-Output "+ $issueDescription"
            $issuesInRelease += $issue
        }
        else {
            # Story sub-tasks are expected to be ignored
            Write-Output "- Ignored: $issueDescription"
        }
    }

    # Look for stories in previous releases to see if they are ready to be released
    Write-Verbose "Searching for unreleased stories from previous releases."
    foreach ($jiraTask in $previousVersionTasks) {
        try {
            $issue = Get-JiraIssue -Key $jiraTask -Credential $credential -Fields $jiraIssueFilter
        }
        catch {
            if ($_.Exception.Message -like "Issue Does Not Exist") {
                Write-Warning "Issue $jiraTask not found."
                continue
            }
            throw "Error while retrieving $($jiraTask): $($_.Exception.Message)`n$($_.ScriptStackTrace)"
        }

        if (-not $issue) {
            Write-Warning "Issue $jiraTask not found."
            continue
        }

        $issueDescription = "$($issue.Key) $($issue.issuetype.name) $($issue.Summary)"

        # Ignore issues that are already released
        if ($issue.fixVersions | Where-Object { $_.name -like "$DefaultPackage *" -and $_.released -eq $true }) {
            continue
        }

        # Only check stories from previous versions
        if ($issue.issuetype.id -in $releaseStoryIssueTypes -and $issue.Status -in $releaseStoryIssueStatuses) {
            # Story commit
            Write-Output "+ $issueDescription"
            $issuesInRelease += $issue
        }
    }
}

$releaseJiraVersion = Get-JiraVersion -Credential $credential -Project $JiraProject -Name $releaseName

if ($releaseJiraVersion) {
    # Include issues already in release to update tags for these also
    $issuesAlreadyInRelease = Get-JiraIssue -Credential $credential -Fields $jiraIssueFilter -Query "fixVersion = $($releaseJiraVersion.ID)"
    foreach ($issue in $issuesAlreadyInRelease) {
        $issueDescription = "$($issue.Key) $($issue.issuetype.name) $($issue.Summary)"

        if ($issue.Key -notin $issuesInRelease.Key) {
            Write-Output "+ Already tagged in release: $issueDescription"
            $issuesInRelease += $issue
        }
    }
}
else {
    # Create new release version in Jira
    if ($PSCmdlet.ShouldProcess($releaseName, "Create new release")) {
        $releaseJiraVersion = New-JiraVersion -Credential $credential -Project $JiraProject -Name $releaseName -StartDate (Get-Date).ToUniversalTime()
    }
}

foreach ($issue in $issuesInRelease) {
    $newFixVersion = @($releaseJiraVersion.name)
    $currentFixVersion = ($issue.fixVersions | Where-Object { $_.name -notlike "$DefaultPackage *" })
    if ($currentFixVersion) {
        $newFixVersion += $currentFixVersion.name
    }

    $setJiraIssueParameters = @{
        Credential = $credential
        Issue = $issue.Key
        FixVersion = $newFixVersion
    }

    # customfield_10018 is nuget package version name in Jira
    if ($issue.issuetype.id -in $issuesWithNugetPackageVersion) {
        $setJiraIssueParameters["Fields"] = @{
            customfield_10018 = $packageName
        }
    }

    if ($PSCmdlet.ShouldProcess($issue.Key, "Set issue version and package")) {
        Set-JiraIssue @setJiraIssueParameters
    }
}

if ($Released.IsPresent) {
    if ($PSCmdlet.ShouldProcess($releaseJiraVersion.name, "Mark release as released")) {
        $null = Set-JiraVersion -Credential $credential -Version $releaseJiraVersion -ReleaseDate (Get-Date).ToUniversalTime() -Released $true
    }
}
