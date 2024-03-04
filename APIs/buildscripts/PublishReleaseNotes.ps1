<#
   .SYNOPSIS
        Creates a new release notes page in wiki with a list of workitems from the specified release in Jira

    .PARAMETER JiraUsername
        Username for Jira user

    .PARAMETER JiraPassword
        Password for Jira user

    .PARAMETER WikiPAT
        PersonalAccessToken for Wiki user

    .PARAMETER ReleaseName
        Name of package, for example "Episerver.Find"

    .PARAMETER ReleaseVersion
        Version of package, for example "14.0.4"

    .PARAMETER JiraProject
        Name of project to create release in

    .PARAMETER ChangeLogPageId
        ID of change log page to create the release notes as child to.

    .PARAMETER JiraUrl
        URL for Jira API

    .PARAMETER WikiUrl
        URL for Confluence API
#>

[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true)]
    [string] $JiraUsername,

    [Parameter(Mandatory = $true)]
    [string] $JiraPassword,

    [Parameter(Mandatory = $true)]
    [string] $WikiPAT,

    [Parameter(Mandatory = $true)]
    [string] $ReleaseName = "EPiServer.Find",

    [Parameter(Mandatory = $true)]
    [string] $ReleaseVersion,
    
    [Parameter(Mandatory = $false)]
    [string] $JiraProject = "FIND",

    [Parameter(Mandatory = $false)]
    [string] $ChangeLogPageId = "2858192210",

    [Parameter(Mandatory = $false)]
    [string] $JiraUrl = "https://jira.sso.episerver.net",

    [Parameter(Mandatory = $false)]
    [string] $WikiUrl = "https://confluence.sso.episerver.net"
)

Import-Module "$PSScriptRoot\JiraPS" -Force
Import-Module "$PSScriptRoot\ConfluencePS" -Force

$pageBodyBuilder = [System.Text.StringBuilder]::new()

function RenderHeader {
    param(
        $header
    )
    $null = $pageBodyBuilder.Append("<h1>");
    $null = $pageBodyBuilder.Append($header);
    $null = $pageBodyBuilder.Append("</h1>");

    Write-Output $header
}

function RenderSubHeader {
    param(
        $subHeader
    )
    $null = $pageBodyBuilder.Append("<h2>");
    $null = $pageBodyBuilder.Append($subHeader);
    $null = $pageBodyBuilder.Append("</h2>");

    Write-Output $subHeader
}
function RenderListStart {
    $null = $pageBodyBuilder.Append("<ul>");
}

function RenderListEnd {
    $null = $pageBodyBuilder.Append("</ul>");
}

function RenderItem {
    param(
        $JiraItem
    )
    $null = $pageBodyBuilder.Append("<li>")
    $null = $pageBodyBuilder.Append("[")
    $null = $pageBodyBuilder.Append("<a href='$($JiraItem.HttpUrl)'>")
    $null = $pageBodyBuilder.Append($JiraItem.Key)
    $null = $pageBodyBuilder.Append("</a>")
    $null = $pageBodyBuilder.Append("]")
    $null = $pageBodyBuilder.Append(" - ")
    $null = $pageBodyBuilder.Append([System.Net.WebUtility]::HtmlEncode($JiraItem.Summary))
    $null = $pageBodyBuilder.Append("</li>")

    Write-Output "[$($JiraItem.Key)] ($($JiraItem.HttpUrl)) $([System.Net.WebUtility]::HtmlEncode($JiraItem.Summary))"
}

$releaseName = "$ReleaseName $ReleaseVersion"

Write-Output "Release name: $releaseName"

$secureJiraPassword = ConvertTo-SecureString -String $JiraPassword -AsPlainText -Force
$jiraCredential = [PSCredential]::new($JiraUsername, $secureJiraPassword)

Set-JiraConfigServer -Server $JiraUrl

Set-ConfluenceInfo -BaseURI $WikiUrl

$version = Get-JiraVersion -Credential $jiraCredential -Project "$JiraProject" -Name "$releaseName"
if (-not $version) {
    throw "Release $releaseName not found in Jira."
}

$releaseDate = Get-Date
if ($version.ReleaseDate) {
    $releaseDate = $version.ReleaseDate
}

$releaseTitle = "$releaseName (" + ($releaseDate).Date.ToString("d MMMM yyyy", [System.Globalization.CultureInfo]::InvariantCulture) + ")"
Write-Output $releaseTitle

$changeLogPage = Get-ConfluencePage -PersonalAccessToken $WikiPAT -ID $ChangeLogPageId
$releasePage = Get-ConfluencePage -PersonalAccessToken $WikiPAT -Query "parent=$ChangeLogPageId AND Title='$releaseTitle'"

if (!$changeLogPage) {
    Write-Warning "Changelog page is not found!"
	return
}

$releaseHeader = "Release Notes - Find - Version $($version.Name)"
if ($releasePage -and $releasePage.Body -like "*$releaseHeader*") {
    Write-Output "$($version.Name) is already in release notes."
    return
}

RenderHeader $releaseHeader

$itemInRelease = @(Get-JiraIssue -Credential $jiraCredential -Query "project=Find AND fixVersion = '$($version.Name)' ORDER BY type ASC")
if ($itemInRelease.Count) {
    $currentType = ""

    foreach ($item in $itemInRelease) {
        if ($item.issueType.name -ine $currentType) {
            if ($currentType -ne "") {
                RenderListEnd
            }

            $currentType = "$($item.issueType.name)"
            RenderSubHeader "$currentType"
            RenderListStart
        }

        RenderItem($item)
    }
    if ($currentType -ne "") {
        RenderListEnd
    }
}
else {
    Write-Warning "Release is empty."
	return
}

$releaseBody = $pageBodyBuilder.ToString()

if ($PSCmdlet.ShouldProcess("Release notes page", "Publish")) {
    if ($releasePage) {
        $releaseBody = $releasePage.Body + $releaseBody
    }
    else {
        $releasePage = New-ConfluencePage -PersonalAccessToken $WikiPAT -Title $releaseTitle -Parent $changeLogPage
    }

    Set-ConfluencePage -PersonalAccessToken $WikiPAT -PageId $releasePage.ID -Body $releaseBody
    Write-Output "Release notes created at: $($releasePage.URL)"
}
else {
    Write-Output $releaseBody
}
