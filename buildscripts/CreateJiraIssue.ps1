<#
    .SYNOPSIS
        Create Jira issue.

    .DESCRIPTION
        Create a Jira issue whenever GitHubAction trigger.

    .PARAMETER Username
        Username for Jira user

    .PARAMETER Password
        Password for Jira user

    .PARAMETER JiraUrl
        URL for Jira API

    .PARAMETER JiraProject
        Name of project to create release in

    .PARAMETER IssueType
        IssueType for Jira issue

     .PARAMETER Summary
        Summary/Title for Jira issue

     .PARAMETER Description
        Description for Jira issue

     .PARAMETER Reporter
        Reporter for Jira issue
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
    [string] $Project = "FIND",

    [Parameter(Mandatory = $false)]
    [string] $IssueType = "Story",

    [Parameter(Mandatory = $false)]
    [string] $Summary,

    [Parameter(Mandatory = $false)]
    [string] $Description,

    [Parameter(Mandatory = $false)]
    [string] $Reporter = "manh.nguyen@episerver.com",

    [Parameter(Mandatory = $false)]
    [string] $CreatedBy,

    [Parameter(Mandatory = $false)]
    [string] $AcceptedBy,

    [Parameter(Mandatory = $false)]
    [string] $IssueNumber
)

Import-Module "$PSScriptRoot\JiraPS" -Force

Set-JiraConfigServer -Server $JiraUrl
$securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force
$credential = [PSCredential]::new($Username, $securePassword)

#Check exists by summary before create a new one
$regex = '(FIND-[0-9]+)'
$found = $Summary -match $regex
if ($found) {
    $jiraKey = $matches[1]
}

if ($jiraKey) {
    Write-Debug "Jira key $jiraKey"
    try {
        Get-JiraIssue -Key $jiraKey -Credential $credential
        Write-Host "The issue '$Summary' had already created with Id [$jiraKey]"
        exit 1
    }
    catch {
        #continue create jira issue
    }
}

$Description = "$Description `n Created by [$CreatedBy] `n Accepted By [$AcceptedBy]"

if ($Summary.Contains("[Feature]")) {
    $IssueType = "Story"
}

if($Summary.Contains("[Bugs]")){
    $IssueType = "Bug"
}

Write-Debug "Creating new Jira issue with priority is P4..."
$result = New-JiraIssue -Project $Project -IssueType $IssueType -Summary "$Summary #$IssueNumber" -Description ($Description) -Reporter $Reporter -Credential $credential -Priority 4
if ($result) {
    Write-Output $result.Key
}
