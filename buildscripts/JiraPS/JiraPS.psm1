#region Dependencies
# Load the ConfluencePS namespace from C#
# if (!("" -as [Type])) {
#     Add-Type -Path (Join-Path $PSScriptRoot JiraPS.Types.cs) -ReferencedAssemblies Microsoft.CSharp, Microsoft.PowerShell.Commands.Utility, System.Management.Automation
# }
# if ($PSVersionTable.PSVersion.Major -lt 5) {
#     Add-Type -Path (Join-Path $PSScriptRoot JiraPS.Attributes.cs) -ReferencedAssemblies Microsoft.CSharp, Microsoft.PowerShell.Commands.Utility, System.Management.Automation
# }

# Load Web assembly when needed
# PowerShell Core has the assembly preloaded
if (!("System.Web.HttpUtility" -as [Type])) {
    Add-Type -AssemblyName "System.Web"
}
# Load System.Net.Http when needed
# PowerShell Core has the assembly preloaded
if (!("System.Net.Http.HttpRequestException" -as [Type])) {
    Add-Type -AssemblyName "System.Net.Http"
}
if (!("System.Net.Http" -as [Type])) {
    Add-Type -Assembly System.Net.Http
}
#region Configuration
$script:serverConfig = ("{0}/AtlassianPS/JiraPS/server_config" -f [Environment]::GetFolderPath('ApplicationData'))

if (-not (Test-Path $script:serverConfig)) {
    $null = New-Item -Path $script:serverConfig -ItemType File -Force
}
$script:JiraServerUrl = [Uri](Get-Content $script:serverConfig)

$script:DefaultContentType = "application/json; charset=utf-8"
$script:DefaultPageSize = 25
$script:DefaultHeaders = @{ "Accept-Charset" = "utf-8" }
# Bug in PSv3's .Net API
if ($PSVersionTable.PSVersion.Major -gt 3) {
    $script:DefaultHeaders["Accept"] = "application/json"
}
$script:PagingContainers = @(
    "comments"
    "dashboards"
    "groups"
    "issues"
    "values"
    "worklogs"
)
$script:SessionTransformationMethod = "ConvertTo-JiraSession"
function Add-JiraFilterPermission {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, DefaultParameterSetName = 'ByInputObject' )]
    # [OutputType( [JiraPS.FilterPermission] )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ByInputObject' )]
        [ValidateNotNullOrEmpty()]
        [PSTypeName('JiraPS.Filter')]
        $Filter,

        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ById')]
        [ValidateNotNullOrEmpty()]
        [UInt32[]]
        $Id,

        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [ValidateSet('Group', 'Project', 'ProjectRole', 'Authenticated', 'Global')]
        [String]$Type,

        [Parameter()]
        [String]$Value,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/permission"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if ($PSCmdlet.ParameterSetName -eq 'ById') {
            $Filter = Get-JiraFilter -Id $Id
        }

        $body = @{
            type = $Type.ToLower()
        }
        switch ($Type) {
            "Group" {
                $body["groupname"] = $Value
            }
            "Project" {
                $body["projectId"] = $Value
            }
            "ProjectRole" {
                $body["projectRoleId"] = $Value
            }
            "Authenticated" { }
            "Global" { }
        }

        foreach ($_filter in $Filter) {
            $parameter = @{
                URI        = $resourceURi -f $_filter.RestURL
                Method     = "POST"
                Body       = ConvertTo-Json $body
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($_filter.Name, "Add Permission [$Type - $Value]")) {
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraFilter -InputObject $_filter -FilterPermissions $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraGroupMember {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [Alias('GroupName')]
        [ValidateNotNullOrEmpty()]
        [Object[]]
        $Group,

        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [Object[]]
        $UserName,
        <#
          #ToDo:CustomClass
          Once we have custom classes, this can also accept ValueFromPipeline
        #>

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group/user?groupname={0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_group in $Group) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_group [$_group]"

            $groupObj = Get-JiraGroup -GroupName $_group -Credential $Credential -ErrorAction Stop
            $groupMembers = (Get-JiraGroupMember -Group $_group -Credential $Credential -ErrorAction Stop).Name

            # At present, it looks like this REST method doesn't support arrays in the Name property...
            # in other words, a single REST call can only add a single group member to a single group.

            # That's kind of annoying.

            # Anyway, this builds a bunch of individual JSON strings with each username in its own Web
            # request, which we'll loop through again in the Process block.
            $users = Resolve-JiraUser -InputObject $UserName -Exact -Credential $Credential

            foreach ($user in $users) {

                if ($groupMembers -notcontains $user.Name) {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] User [$($user.Name)] is not already in group [$_group]. Adding user."

                    $parameter = @{
                        URI        = $resourceURi -f $groupObj.Name
                        Method     = "POST"
                        Body       = ConvertTo-Json -InputObject @{ 'name' = $user.Name }
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    if ($PSCmdlet.ShouldProcess($GroupName, "Adding user '$($user.Name)'.")) {
                        $result = Invoke-JiraMethod @parameter
                    }
                }
                else {
                    $errorMessage = @{
                        Category         = "ResourceExists"
                        CategoryActivity = "Adding [$user] to [$_group]"
                        Message          = "User [$user] is already a member of group [$_group]"
                    }
                    Write-Error @errorMessage
                }
            }

            if ($PassThru) {
                Write-Output (ConvertTo-JiraGroup -InputObject $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraIssueAttachment {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,
        <#
          #ToDo:CustomClass
          Once we have custom classes, this can also accept ValueFromPipeline
        #>

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateScript(
            {
                if (-not (Test-Path $_ -PathType Leaf)) {
                    $exception = ([System.ArgumentException]"File not found") #fix code highlighting]
                    $errorId = 'ParameterValue.FileNotFound'
                    $errorCategory = 'ObjectNotFound'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "No file could be found with the provided path '$_'."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('InFile', 'FullName', 'Path')]
        [String[]]
        $FilePath,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/attachments"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if (@($Issue).Count -ne 1) {
            $exception = ([System.ArgumentException]"invalid Issue provided")
            $errorId = 'ParameterValue.JiraIssue'
            $errorCategory = 'InvalidArgument'
            $errorTarget = $_
            $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
            $errorItem.ErrorDetails = "Only one Issue can be provided at a time."
            $PSCmdlet.ThrowTerminatingError($errorItem)
        }

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        foreach ($file in $FilePath) {
            $file = Resolve-FilePath -Path $file

            $enc = [System.Text.Encoding]::GetEncoding("iso-8859-1")
            $boundary = [System.Guid]::NewGuid().ToString()

            $fileName = Split-Path -Path $file -Leaf
            $readFile = [System.IO.File]::ReadAllBytes($file)
            $fileEnc = $enc.GetString($readFile)

            $bodyLines = @'
--{0}
Content-Disposition: form-data; name="file"; filename="{1}"
Content-Type: application/octet-stream

{2}
--{0}--
'@ -f $boundary, $fileName, $fileEnc

            $headers = @{
                'X-Atlassian-Token' = 'nocheck'
                'Content-Type'      = "multipart/form-data; boundary=`"$boundary`""
            }

            $parameter = @{
                URI        = $resourceURi -f $issueObj.RestURL
                Method     = "POST"
                Body       = $bodyLines
                Headers    = $headers
                RawBody    = $true
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($IssueObj.Key, "Adding attachment '$($fileName)'.")) {
                $rawResult = Invoke-JiraMethod @parameter

                if ($PassThru) {
                    Write-Output (ConvertTo-JiraAttachment -InputObject $rawResult)
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraIssueComment {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [String]
        $Comment,

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [ValidateSet('All Users', 'Developers', 'Administrators')]
        [String]
        $VisibleRole = 'All Users',

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/comment"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        $requestBody = @{
            'body' = $Comment
        }

        # If the visible role should be all users, the visibility block shouldn't be passed at
        # all. JIRA returns a 500 Internal Server Error if you try to pass this block with a
        # value of "All Users".
        if ($VisibleRole -ne 'All Users') {
            $requestBody.visibility = @{
                'type'  = 'role'
                'value' = $VisibleRole
            }
        }

        $parameter = @{
            URI        = $resourceURi -f $issueObj.RestURL
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($issueObj.Key)) {
            $rawResult = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraComment -InputObject $rawResult)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraIssueLink {
# .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object[]]
        $Issue,

        [Parameter( Mandatory )]
        [ValidateScript(
            {
                $objectProperties = Get-Member -InputObject $_ -MemberType *Property
                if (-not(
                        ($objectProperties.Name -contains "type") -and
                        (($objectProperties.Name -contains "outwardIssue") -or ($objectProperties.Name -contains "inwardIssue"))
                    )) {
                    $exception = ([System.ArgumentException]"Invalid Parameter") #fix code highlighting]
                    $errorId = 'ParameterProperties.Incomplete'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "The IssueLink provided does not contain the information needed."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $IssueLink,

        [String]
        $Comment,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issueLink"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_issue in $Issue) {
            # Find the proper object for the Issue
            $issueObj = Resolve-JiraIssueObject -InputObject $_issue -Credential $Credential

            foreach ($_issueLink in $IssueLink) {
                if ($_issueLink.inwardIssue) {
                    $inwardIssue = @{ key = $_issueLink.inwardIssue.key }
                }
                else {
                    $inwardIssue = @{ key = $issueObj.key }
                }

                if ($_issueLink.outwardIssue) {
                    $outwardIssue = @{ key = $_issueLink.outwardIssue.key }
                }
                else {
                    $outwardIssue = @{ key = $issueObj.key }
                }

                $body = @{
                    type         = @{ name = $_issueLink.type.name }
                    inwardIssue  = $inwardIssue
                    outwardIssue = $outwardIssue
                }

                if ($Comment) {
                    $body.comment = @{ body = $Comment }
                }

                $parameter = @{
                    URI        = $resourceURi
                    Method     = "POST"
                    Body       = ConvertTo-Json -InputObject $body
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                if ($PSCmdlet.ShouldProcess($issueObj.Key)) {
                    Invoke-JiraMethod @parameter
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraIssueWatcher {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [String[]]
        $Watcher,
        <#
          #ToDo:CustomClass
          Once we have custom classes, this can also accept ValueFromPipeline
        #>

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/watchers"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        foreach ($_watcher in $Watcher) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_watcher]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_watcher [$_watcher]"

            $parameter = @{
                URI        = $resourceURi -f $issueObj.RestURL
                Method     = "POST"
                Body       = '"{0}"' -f $_watcher
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($issueObj.Key, "Adding user '$_watcher' as watcher.")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Add-JiraIssueWorklog {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [String]
        $Comment,

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [TimeSpan]
        $TimeSpent,

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [DateTime]
        $DateStarted,

        [ValidateSet('All Users', 'Developers', 'Administrators')]
        [String]
        $VisibleRole = 'All Users',

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/worklog"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        if (-not $issueObj) {
            $errorMessage = @{
                Category         = "ObjectNotFound"
                CategoryActivity = "Searching for Issue"
                Message          = "Invalid Issue provided."
            }
            Write-Error @errorMessage
        }

        # Harmonize DateStarted:
        # `Get-Date -Date "01.01.2000"` does not return the local timezone
        # which is required by the API
        $DateStarted = [DateTime]::new($DateStarted.Ticks, 'Local')

        $requestBody = @{
            'comment'          = $Comment
            # We need to fix the date with a RegEx replace because the API does not like:
            # * miliseconds with more than 3 digits
            # * `:` in the TimeZone
            'started'          = $DateStarted.ToString("o") -replace "\.(\d{3})\d*([\+\-]\d{2}):", ".`$1`$2"
            'timeSpentSeconds' = $TimeSpent.TotalSeconds.ToString()
        }

        # If the visible role should be all users, the visibility block shouldn't be passed at
        # all. JIRA returns a 500 Internal Server Error if you try to pass this block with a
        # value of "All Users".
        if ($VisibleRole -ne 'All Users') {
            $requestBody.visibility = @{
                'type'  = 'role'
                'value' = $VisibleRole
            }
        }

        $parameter = @{
            URI        = $resourceURi -f $issueObj.RestURL
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($issueObj.Key)) {
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraWorklogitem -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Find-JiraFilter {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName='ByAccountId', SupportsPaging )]
    param(
        [Parameter(ValueFromPipeline,ValueFromPipelineByPropertyName)]
        [string[]]$Name,

        [Parameter(ParameterSetName='ByAccountId',ValueFromPipelineByPropertyName)]
        [string]$AccountId,

        [Parameter(ParameterSetName='ByOwner',ValueFromPipelineByPropertyName)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.User" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraUser'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Owner. Expected [JiraPS.User] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('UserName')]
        [Object]
        $Owner,

        [Parameter(ValueFromPipelineByPropertyName)]
        [string]$GroupName,

        [Parameter(ValueFromPipelineByPropertyName)]
        [ValidateScript(
            {
                if (("JiraPS.Project" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraProject'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Project. Expected [JiraPS.Project] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $Project,

        [Validateset('description','favourite','favouritedCount','jql','owner','searchUrl','sharePermissions','subscriptions','viewUrl')]
        [String[]]
        $Fields = @('description','favourite','favouritedCount','jql','owner','searchUrl','sharePermissions','subscriptions','viewUrl'),

        [Validateset('description','favourite_count','is_favourite','id','name','owner')]
        [string]$Sort,

        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $searchURi = "$server/rest/api/2/filter/search"

        [String]$Fields = $Fields -join ','
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"
        $parameter = @{
            URI          = $searchURi
            Method       = 'GET'
            GetParameter = @{
                expand = $Fields
            }
            Paging       = $true
            Credential   = $Credential
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey('AccountId')) {
            $parameter['GetParameter']['accountId'] = $AccountId
        }
        elseif ($PSCmdlet.ParameterSetName -eq 'ByOwner') {
            $userObj = Get-JiraUser -InputObject $Owner -Credential $Credential -ErrorAction Stop
            $parameter['GetParameter']['accountId'] = $userObj.AccountId
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey('GroupName')) {
            $parameter['GetParameter']['groupName'] = $GroupName
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey('Project')) {
            $projectObj = Get-JiraProject -Project $Project -Credential $Credential -ErrorAction Stop
            $parameter['GetParameter']['projectId'] = $projectObj.Id
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey('Sort')) {
            $parameter['GetParameter']['orderBy'] = $Sort
        }
        # Paging
        ($PSCmdlet.PagingParameters | Get-Member -MemberType Property).Name | ForEach-Object {
            $parameter[$_] = $PSCmdlet.PagingParameters.$_
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey('Name')) {
            foreach($_name in $Name) {
                $parameter['GetParameter']['filterName'] = $_name
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"

                Write-Output (Invoke-JiraMethod @parameter | ConvertTo-JiraFilter)
            }
        }
        else {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"

            Write-Output (Invoke-JiraMethod @parameter | ConvertTo-JiraFilter)
        }

    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Format-Jira {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [OutputType([System.String])]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromRemainingArguments )]
        [ValidateNotNull()]
        [PSObject[]]
        $InputObject,

        [Object[]]
        $Property
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $headers = New-Object -TypeName System.Collections.ArrayList
        $thisLine = New-Object -TypeName System.Text.StringBuilder
        $allText = New-Object -TypeName System.Text.StringBuilder

        $headerDefined = $false

        $n = [System.Environment]::NewLine

        if ($Property) {
            if ($Property -eq '*') {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] -Property * was passed. Adding all properties."
            }
            else {

                foreach ($p in $Property) {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding header [$p]"
                    [void] $headers.Add($p.ToString())
                }

                $headerString = "||$(($headers.ToArray()) -join '||')||"
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Full header: [$headerString]"
                [void] $allText.Append($headerString)
                $headerDefined = $true
            }
        }
        else {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Property parameter was not specified. Checking first InputObject for property names."
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($i in $InputObject) {
            if (-not ($headerDefined)) {
                # This should only be called if Property was not supplied and this is the first object in the InputObject array.
                if ($Property -and $Property -eq '*') {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding all properties from object [$i]"
                    $allProperties = Get-Member -InputObject $i -MemberType '*Property'
                    foreach ($a in $allProperties) {
                        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding header [$($a.Name)]"
                        [void] $headers.Add($a.Name)
                    }
                }
                else {

                    # TODO: find a way to format output objects based on PowerShell's own Format-Table
                    # Identify default table properties if possible and use them to create a Jira table

                    if ($i.PSStandardMembers.DefaultDisplayPropertySet) {
                        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Identifying default properties for object [$i]"
                        $propertyNames = $i.PSStandardMembers.DefaultDisplayPropertySet.ReferencedPropertyNames
                        foreach ($p in $propertyNames) {
                            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding header [$p]"
                            [void] $headers.Add($p)
                        }
                    }
                    else {
                        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] No default format data exists for object [$i] (type=[$($i.GetType())]). All properties will be used."
                        $allProperties = Get-Member -InputObject $i -MemberType '*Property'
                        foreach ($a in $allProperties) {
                            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding header [$($a.Name)]"
                            [void] $headers.Add($a.Name)
                        }
                    }
                }

                $headerString = "||$(($headers.ToArray()) -join '||')||"
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Full header: [$headerString]"
                [void] $allText.Append($headerString)
                $headerDefined = $true
            }

            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Processing object [$i]"
            [void] $thisLine.Clear()
            [void] $thisLine.Append("$n|")

            foreach ($h in $headers) {
                $value = $InputObject.$h
                if ($value) {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding property (name=[$h], value=[$value])"
                    [void] $thisLine.Append("$value|")
                }
                else {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Property [$h] does not exist on this object."
                    [void] $thisLine.Append(' |')
                }
            }

            $thisLineString = $thisLine.ToString()
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Completed line: [$thisLineString]"
            [void] $allText.Append($thisLineString)
        }
    }

    end {
        Write-Output $allText.ToString()

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraComponent {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding(DefaultParameterSetName = 'ByID')]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ByProject' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Project" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraProject'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Project] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $Project,
        <#
          #ToDo:CustomClass
          Once we have custom classes, these two parameters can be one
        #>

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByID' )]
        [Alias("Id")]
        [Int[]]
        $ComponentId,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            "ByProject" {
                foreach ($_project in $Project) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_project]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_project [$_project]"

                    if ($_project -isnot [string]) {
                        $_project = $_project.Key
                    }
                    $parameter = @{
                        URI        = $resourceURi -f "/project/$_project/components"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraComponent -InputObject $result)
                }
            }
            "ByID" {
                foreach ($_id in $ComponentId) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

                    $parameter = @{
                        URI        = $resourceURi -f "/component/$_id"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraComponent -InputObject $result)
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraConfigServer {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [OutputType([System.String])]
    param()

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        return ($script:JiraServerUrl -replace "\/$", "")
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraField {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = '_All' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = '_Search' )]
        [String[]]
        $Field,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/field"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            '_All' {
                $parameter = @{
                    URI        = $resourceURi
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraField -InputObject $result)
            }
            '_Search' {
                foreach ($_field in $Field) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_field]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_field [$_field]"

                    $allFields = Get-JiraField -Credential $Credential

                    Write-Output ($allFields | Where-Object -FilterScript {($_.Id -eq $_field) -or ($_.Name -like $_field)})
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraFilter {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding(DefaultParameterSetName = 'ByFilterID')]
    param(
        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByFilterID' )]
        [String[]]
        $Id,
        <#
          #ToDo:CustomClass
          Once we have custom classes for the module,
          this can use ValueFromPipelineByPropertyName
          and we will no longer need the InputObject
        #>

        [Parameter( Mandatory, ValueFromPipeline, ParameterSetName = 'ByInputObject' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Filter" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraFilter'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Filter. Expected [JiraPS.Filter] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $InputObject,

        [Parameter( Mandatory, ParameterSetName = 'MyFavorite' )]
        [Alias('Favourite')]
        [Switch]
        $Favorite,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/filter/{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            "ByFilterID" {
                foreach ($_id in $Id) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

                    $parameter = @{
                        URI        = $resourceURi -f $_id
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraFilter -InputObject $result)
                }
            }
            "ByInputObject" {
                foreach ($object in $InputObject) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$object]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$object [$object]"

                    if ((Get-Member -InputObject $object).TypeName -eq 'JiraPS.Filter') {
                        $thisId = $object.ID
                    }
                    else {
                        $thisId = $object.ToString()
                        Write-Verbose "[$($MyInvocation.MyCommand.Name)] ID is assumed to be [$thisId] via ToString()"
                    }

                    Write-Output (Get-JiraFilter -Id $thisId -Credential $Credential)
                }
            }
            "MyFavorite" {
                $parameter = @{
                    URI        = $resourceURi -f "favourite"
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraFilter -InputObject $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraFilterPermission {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = 'ById' )]
    # [OutputType( [JiraPS.FilterPermission] )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ByInputObject' )]
        [ValidateNotNullOrEmpty()]
        [PSTypeName('JiraPS.Filter')]
        $Filter,

        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ById')]
        [ValidateNotNullOrEmpty()]
        [UInt32[]]
        $Id,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $resourceURi = "{0}/permission"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if ($PSCmdlet.ParameterSetName -eq 'ById') {
            $Filter = Get-JiraFilter -Id $Id
        }

        foreach ($_filter in $Filter) {
            $parameter = @{
                URI        = $resourceURi -f $_filter.RestURL
                Method     = "GET"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraFilter -InputObject $_filter -FilterPermissions $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraGroup {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [Alias('Name')]
        [String[]]
        $GroupName,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group?groupname={0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($group in $GroupName) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$group [$group]"

            $escapedGroupName = ConvertTo-URLEncoded $group

            $parameter = @{
                URI        = $resourceURi -f $escapedGroupName
                Method     = "GET"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraGroup -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraGroupMember {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsPaging )]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Group" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraGroup'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Group. Expected [JiraPS.Group] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $Group,

        [Switch]
        $IncludeInactive,

        [UInt32]
        $StartIndex = 0,

        [UInt32]
        $MaxResults,

        [UInt32]
        $PageSize = $script:DefaultPageSize,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group/member"

        if ($PageSize -gt 50) {
            Write-Warning "JIRA's API may not properly support MaxResults values higher than 50 for this method. If you receive inconsistent results, do not pass the MaxResults parameter to this function to return all results."
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $groupObj = Get-JiraGroup -GroupName $Group -Credential $Credential -ErrorAction Stop

        foreach ($_group in $groupObj) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_group [$_group]"

            $parameter = @{
                URI          = $resourceURi
                Method       = "GET"
                GetParameter = @{
                    groupname  = $_group.Name
                    maxResults = $PageSize
                }
                OutputType   = "JiraUser"
                Paging       = $true
                Credential   = $Credential
            }
            if ($IncludeInactive) {
                $parameter["includeInactiveUsers"] = $true
            }

            # Paging
            ($PSCmdlet.PagingParameters | Get-Member -MemberType Property).Name | ForEach-Object {
                $parameter[$_] = $PSCmdlet.PagingParameters.$_
            }
            # Make `SupportsPaging` be backwards compatible
            if ($StartIndex) {
                Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-StartIndex' has been marked as deprecated. For more information, plase read the help."
                $parameter["Skip"] = $StartIndex
            }
            if ($MaxResults) {
                Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-MaxResults' has been marked as deprecated. For more information, plase read the help."
                $parameter["First"] = $MaxResults
            }

            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            Invoke-JiraMethod @parameter
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssue {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsPaging, DefaultParameterSetName = 'ByIssueKey' )]
    param(
        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByIssueKey' )]
        [ValidateNotNullOrEmpty()]
        [Alias('Issue')]
        [String[]]
        $Key,

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByInputObject' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $InputObject,
        <#
          #ToDo:Deprecate
          This is not necessary if $Key uses ValueFromPipelineByPropertyName
          #ToDo:CustomClass
          Once we have custom classes, this check can be done with Type declaration
        #>

        [Parameter( Mandatory, ParameterSetName = 'ByJQL' )]
        [Alias('JQL')]
        [String]
        $Query,

        [Parameter( Mandatory, ParameterSetName = 'ByFilter' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Filter" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraFilter'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Filter. Expected [JiraPS.Filter] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $Filter,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [String[]]
        $Fields = "*all",

        [Parameter( ParameterSetName = 'ByJQL' )]
        [Parameter( ParameterSetName = 'ByFilter' )]
        [UInt32]
        $StartIndex = 0,

        [Parameter( ParameterSetName = 'ByJQL' )]
        [Parameter( ParameterSetName = 'ByFilter' )]
        [UInt32]
        $MaxResults = 0,

        [Parameter( ParameterSetName = 'ByJQL' )]
        [Parameter( ParameterSetName = 'ByFilter' )]
        [UInt32]
        $PageSize = $script:DefaultPageSize,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $searchURi = "$server/rest/api/2/search"
        $resourceURi = "$server/rest/api/2/issue/{0}"

        [String]$Fields = $Fields -join ","
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            'ByIssueKey' {
                foreach ($_key in $Key) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_key]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_key [$_key]"

                    $getParameter = @{ expand = "transitions" }
                    if ($Fields) {
                        $getParameter["fields"] = $Fields
                    }

                    $parameter = @{
                        URI          = $resourceURi -f $_key
                        Method       = "GET"
                        GetParameter = $getParameter
                        Credential   = $Credential
                    }

                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraIssue -InputObject $result)
                }
            }
            'ByInputObject' {
                # Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-InputObject' has been marked as deprecated."
                foreach ($_issue in $InputObject) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issue]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_issue [$_issue]"

                    Write-Output (Get-JiraIssue -Key $_issue.Key -Fields $Fields -Credential $Credential)
                }
            }
            'ByJQL' {
                $parameter = @{
                    URI          = $searchURi
                    Method       = "GET"
                    GetParameter = @{
                        jql           = (ConvertTo-URLEncoded $Query)
                        validateQuery = $true
                        expand        = "transitions"
                        maxResults    = $PageSize

                    }
                    OutputType   = "JiraIssue"
                    Paging       = $true
                    Credential   = $Credential
                }
                if ($Fields) {
                    $parameter["GetParameter"]["fields"] = $Fields
                }
                # Paging
                ($PSCmdlet.PagingParameters | Get-Member -MemberType Property).Name | ForEach-Object {
                    $parameter[$_] = $PSCmdlet.PagingParameters.$_
                }
                # Make `SupportsPaging` be backwards compatible
                if ($StartIndex) {
                    Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-StartIndex' has been marked as deprecated. For more information, plase read the help."
                    $parameter["Skip"] = $StartIndex
                }
                if ($MaxResults) {
                    Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-MaxResults' has been marked as deprecated. For more information, plase read the help."
                    $parameter["First"] = $MaxResults
                }


                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                Invoke-JiraMethod @parameter
            }
            'ByFilter' {
                $filterObj = (Get-JiraFilter -InputObject $Filter -Credential $Credential -ErrorAction Stop).searchurl
                <#
                  #ToDo:CustomClass
                  Once we have custom classes, this will no longer be necessary
                #>

                $parameter = @{
                    URI          = $filterObj
                    Method       = "GET"
                    GetParameter = @{
                        validateQuery = $true
                        expand        = "transitions"
                        maxResults    = $PageSize
                    }
                    OutputType   = "JiraIssue"
                    Paging       = $true
                    Credential   = $Credential

                }
                if ($Fields) {
                    $parameter["GetParameter"]["fields"] = $Fields
                }
                # Paging
                ($PSCmdlet.PagingParameters | Get-Member -MemberType Property).Name | ForEach-Object {
                    $parameter[$_] = $PSCmdlet.PagingParameters.$_
                }
                # Make `SupportsPaging` be backwards compatible
                if ($StartIndex) {
                    Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-StartIndex' has been marked as deprecated. For more information, plase read the help."
                    $parameter["Skip"] = $StartIndex
                }
                if ($MaxResults) {
                    Write-Warning "[$($MyInvocation.MyCommand.Name)] The parameter '-MaxResults' has been marked as deprecated. For more information, plase read the help."
                    $parameter["First"] = $MaxResults
                }

                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueAttachment {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = [System.Management.Automation.ErrorCategory]::InvalidArgument
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [String]
        $FileName,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        if ($issueObj.Attachment) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Found Attachments on the Issue."
            if ($FileName) {
                $attachments = $issueObj.Attachment | Where-Object {$_.Filename -like $FileName}
            }
            else {
                $attachments = $issueObj.Attachment
            }

            ConvertTo-JiraAttachment -InputObject $attachments
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}


function Get-JiraIssueAttachmentFile {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [OutputType([Bool])]
    param (
        [Parameter( Mandatory, ValueFromPipeline )]
        [PSTypeName('JiraPS.Attachment')]
        $Attachment,

        [ValidateScript(
            {
                if (-not (Test-Path $_)) {
                    $errorItem = [System.Management.Automation.ErrorRecord]::new(
                        ([System.ArgumentException]"Path not found"),
                        'ParameterValue.FileNotFound',
                        [System.Management.Automation.ErrorCategory]::ObjectNotFound,
                        $_
                    )
                    $errorItem.ErrorDetails = "Invalid path '$_'."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [String]
        $Path,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_Attachment in $Attachment) {
            if ($Path) {
                $filename = Join-Path $Path $_Attachment.Filename
            }
            else {
                $filename = $_Attachment.Filename
            }

            $iwParameters = @{
                Uri        = $_Attachment.Content
                Method     = 'Get'
                Headers    = @{"Accept" = $_Attachment.MimeType}
                OutFile    = $filename
                Credential = $Credential
            }

            $result = Invoke-JiraMethod @iwParameters
            (-not $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function ended"
    }
}

function Get-JiraIssueComment {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        $parameter = @{
            URI          = "{0}/comment" -f $issueObj.RestURL
            Method       = "GET"
            GetParameter = @{
                maxResults = $PageSize
            }
            OutputType   = "JiraComment"
            Paging       = $true
            Credential   = $Credential
        }

        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        Invoke-JiraMethod @parameter
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueCreateMetadata {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory )]
        [String]
        $Project,

        [Parameter( Mandatory )]
        [String]
        $IssueType,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        #resourceURi = "$server/rest/api/2/issue/createmeta?projectIds={0}&issuetypeIds={1}&expand=projects.issuetypes.fields"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $projectObj = Get-JiraProject -Project $Project -Credential $Credential -ErrorAction Stop
        $issueTypeObj = $projectObj.IssueTypes | Where-Object -FilterScript {$_.Id -eq $IssueType -or $_.Name -eq $IssueType}
        $fieldsResourceURi = "$server/rest/api/2/issue/createmeta/$($projectObj.Id)/issuetypes/$($issueTypeObj.Id)"

        if ($null -eq $issueTypeObj.Id)
        {
            $errorMessage = @{
                Category         = "InvalidResult"
                CategoryActivity = "Validating parameters"
                Message          = "No issue types were found in the project [$Project] for the given issue type [$IssueType]. Use Get-JiraIssueType for more details."
            }
            Write-Error @errorMessage
        }

        $parameter = @{
            URI        = $fieldsResourceURi
            Method     = "GET"
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        $result = Invoke-JiraMethod @parameter

        if ($result) {
            if (@($result.projects).Count -eq 0) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "No projects were found for the given project [$Project]. Use Get-JiraProject for more details."
                }
                Write-Error @errorMessage
            }
            elseif (@($result.projects).Count -gt 1) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "Multiple projects were found for the given project [$Project]. Refine the parameters to return only one project."
                }
                Write-Error @errorMessage
            }

            if (@($result.projects.issuetypes) -eq 0) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "No issue types were found for the given issue type [$IssueType]. Use Get-JiraIssueType for more details."
                }
                Write-Error @errorMessage
            }
            elseif (@($result.projects.issuetypes).Count -gt 1) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "Multiple issue types were found for the given issue type [$IssueType]. Refine the parameters to return only one issue type."
                }
                Write-Error @errorMessage
            }

            # Write-Output (ConvertTo-JiraCreateMetaField -InputObject $result)
        }
        else {
            $exception = ([System.ArgumentException]"No results")
            $errorId = 'IssueMetadata.ObjectNotFound'
            $errorCategory = 'ObjectNotFound'
            $errorTarget = $Project
            $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
            $errorItem.ErrorDetails = "No metadata found for project $Project and issueType $IssueType."
            Throw $errorItem
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueEditMetadata {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory )]
        [String]
        $Issue,
        <#
          #ToDo:CustomClass
          Once we have custom classes, this should be a JiraPS.Issue
        #>

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issue/{0}/editmeta"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $parameter = @{
            URI        = $resourceURi -f $Issue
            <#
              #ToDo:CustomClass
              When the Input is typecasted to a JiraPS.Issue, the `self` of the issue can be used
            #>
            Method     = "GET"
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        $result = Invoke-JiraMethod @parameter

        Write-Debug ($result | Out-String)

        if ($result) {
            if (@($result.fields.projects).Count -eq 0) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "No projects were found for the given project [$Project]. Use Get-JiraProject for more details."
                }
                Write-Error @errorMessage
            }
            elseif (@($result.fields.projects).Count -gt 1) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "Multiple projects were found for the given project [$Project]. Refine the parameters to return only one project."
                }
                Write-Error @errorMessage
            }

            if (@($result.fields.projects.issuetypes) -eq 0) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "No issue types were found for the given issue type [$IssueType]. Use Get-JiraIssueType for more details."
                }
                Write-Error @errorMessage
            }
            elseif (@($result.fields.projects.issuetypes).Count -gt 1) {
                $errorMessage = @{
                    Category         = "InvalidResult"
                    CategoryActivity = "Validating response"
                    Message          = "Multiple issue types were found for the given issue type [$IssueType]. Refine the parameters to return only one issue type."
                }
                Write-Error @errorMessage
            }

            Write-Output (ConvertTo-JiraEditMetaField -InputObject $result)
        }
        else {
            $exception = ([System.ArgumentException]"No results")
            $errorId = 'IssueMetadata.ObjectNotFound'
            $errorCategory = 'ObjectNotFound'
            $errorTarget = $Project
            $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
            $errorItem.ErrorDetails = "No metadata found for project $Project and issueType $IssueType."
            Throw $errorItem
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueLink {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [Int[]]
        $Id,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issueLink/{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Validate input object from Pipeline
        if (($_) -and ("JiraPS.IssueLink" -notin $_.PSObject.TypeNames)) {
            $exception = ([System.ArgumentException]"Invalid Parameter")
            $errorId = 'ParameterProperties.WrongObjectType'
            $errorCategory = [System.Management.Automation.ErrorCategory]::InvalidArgument
            $errorTarget = $Id
            $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
            $errorItem.ErrorDetails = "The IssueLink provided did not match the constraints."
            $PSCmdlet.ThrowTerminatingError($errorItem)
        }

        foreach ($_id in $Id) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

            $parameter = @{
                URI        = $resourceURi -f $_id
                Method     = "GET"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraIssueLink -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueLinkType {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = '_All' )]
    param(
        [Parameter( Position = 0, Mandatory, ParameterSetName = '_Search' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.IssueLinkType" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String])) -and (($_ -isnot [Int]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssueLinkType'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for IssueLinkType. Expected [JiraPS.IssueLinkType], [String] or [Int], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $LinkType,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issueLinkType{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            '_All' {
                $parameter = @{
                    URI        = $resourceURi -f ""
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraIssueLinkType -InputObject $result.issueLinkTypes)
            }
            '_Search' {
                # If the link type provided is an int, we can assume it's an ID number.
                # If it's a String, it's probably a name, though, and there isn't an API call to look up a link type by name.
                if ($LinkType -is [Int]) {
                    $parameter = @{
                        URI        = $resourceURi -f "/$LinkType"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraIssueLinkType -InputObject $result)
                }
                else {
                    Write-Output (Get-JiraIssueLinkType -Credential $Credential | Where-Object { $_.Name -like $LinkType })
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueType {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = '_All' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = '_Search' )]
        [String[]]
        $IssueType,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issuetype"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            '_All' {
                $parameter = @{
                    URI        = $resourceURi
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraIssueType -InputObject $result)
            }
            '_Search' {
                foreach ($_issueType in $IssueType) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issueType]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_issueType [$_issueType]"

                    $allIssueTypes = Get-JiraIssueType -Credential $Credential

                    Write-Output ($allIssueTypes | Where-Object -FilterScript {$_.Id -eq $_issueType})
                    Write-Output ($allIssueTypes | Where-Object -FilterScript {$_.Name -like $_issueType})
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraIssueWatcher {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlight]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        foreach ($issue in $issueObj) {
            $parameter = @{
                URI        = "{0}/watchers" -f $issue.RestURL
                Method     = "GET"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($result = Invoke-JiraMethod @parameter) {
                Write-Output $result.watchers
                # TODO: are these users?
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraPriority {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = '_All' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = '_Search' )]
        [Int[]]
        $Id,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/priority{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            '_All' {
                $parameter = @{
                    URI        = $resourceURi -f ""
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraPriority -InputObject $result)
            }
            '_Search' {
                foreach ($_id in $Id) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

                    $parameter = @{
                        URI        = $resourceURi -f "/$_id"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraPriority -InputObject $result)
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraProject {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = '_All' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = '_Search' )]
        [String[]]
        $Project,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/project{0}?expand=description,lead,issueTypes,url,projectKeys"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            '_All' {
                $parameter = @{
                    URI        = $resourceURi -f ""
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraProject -InputObject $result)
            }
            '_Search' {
                foreach ($_project in $Project) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_project]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_project [$_project]"

                    $parameter = @{
                        URI        = $resourceURi -f "/$($_project)"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraProject -InputObject $result)
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraRemoteLink {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias("Key")]
        [Object]
        $Issue,

        [Int]
        $LinkId,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_issue in $Issue) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issue]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_issue [$_issue]"

            # Find the proper object for the Issue
            $issueObj = Resolve-JiraIssueObject -InputObject $_issue -Credential $Credential

            $urlAppendix = ""
            if ($LinkId) {
                $urlAppendix = "/$LinkId"
            }

            $parameter = @{
                URI        = "{0}/remotelink{1}" -f $issueObj.RestUrl, $urlAppendix
                Method     = "GET"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraLink -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraServerInformation {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/serverInfo"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $parameter = @{
            URI        = $resourceURi
            Method     = "GET"
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        $result = Invoke-JiraMethod @parameter

        Write-Output (ConvertTo-JiraServerInfo -InputObject $result)
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

New-Alias -Name "Get-JiraServerInfo" -Value "Get-JiraServerInformation" -ErrorAction SilentlyContinue

function Get-JiraSession {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param()

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if ($MyInvocation.MyCommand.Module.PrivateData -and $MyInvocation.MyCommand.Module.PrivateData.Session) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Using Session saved in PrivateData"
            Write-Output $MyInvocation.MyCommand.Module.PrivateData.Session
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraUser {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = 'Self' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipelineByPropertyName, ParameterSetName = 'ByUserName' )]
        [AllowEmptyString()]
        [Alias('User', 'Name')]
        [String[]]
        $UserName,

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByInputObject' )]
        [Object[]] $InputObject,

        [Parameter( ParameterSetName = 'ByInputObject' )]
        [Parameter( ParameterSetName = 'ByUserName' )]
        [Switch]$Exact,

        [Switch]
        $IncludeInactive,

        [Parameter( ParameterSetName = 'ByUserName' )]
        [ValidateRange(1, 1000)]
        [UInt32]
        $MaxResults = 50,

        [Parameter( ParameterSetName = 'ByUserName' )]
        [ValidateNotNullOrEmpty()]
        [UInt64]
        $Skip = 0,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $selfResourceUri = "$server/rest/api/2/myself"
        $searchResourceUri = "$server/rest/api/2/user/search?username={0}"
        $exactResourceUri = "$server/rest/api/2/user?username={0}"

        if ($IncludeInactive) {
            $searchResourceUri += "&includeInactive=true"
        }
        if ($MaxResults) {
            $searchResourceUri += "&maxResults=$MaxResults"
        }
        if ($Skip) {
            $searchResourceUri += "&startAt=$Skip"
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $ParameterSetName = ''
        switch ($PsCmdlet.ParameterSetName) {
            'ByInputObject' { $UserName = $InputObject.Name; $ParameterSetName = 'ByUserName'; $Exact = $true }
            'ByUserName' { $ParameterSetName = 'ByUserName' }
            'Self' { $ParameterSetName = 'Self' }
        }

        switch ($ParameterSetName) {
            "Self" {
                $resourceURi = $selfResourceUri

                $parameter = @{
                    URI        = $resourceURi
                    Method     = "GET"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                $result = Invoke-JiraMethod @parameter

                Get-JiraUser -UserName $result.Name -Exact
            }
            "ByInputObject" {
                $UserName = $InputObject.Name

                $PsCmdlet.ParameterSetName = "ByUserName"
            }
            "ByUserName" {
                $resourceURi = if ($Exact) { $exactResourceUri } else { $searchResourceUri }

                foreach ($user in $UserName) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$user]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$user [$user]"

                    $parameter = @{
                        URI        = $resourceURi -f $user
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    if ($users = Invoke-JiraMethod @parameter) {
                        foreach ($item in $users) {
                            $parameter = @{
                                URI        = "{0}&expand=groups" -f $item.self
                                Method     = "GET"
                                Credential = $Credential
                            }
                            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                            $result = Invoke-JiraMethod @parameter

                            Write-Output (ConvertTo-JiraUser -InputObject $result)
                        }
                    }
                    else {
                        $errorMessage = @{
                            Category         = "ObjectNotFound"
                            CategoryActivity = "Searching for user"
                            Message          = "No results when searching for user $user"
                        }
                        Write-Error @errorMessage
                    }
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Get-JiraVersion {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsPaging, DefaultParameterSetName = 'byId' )]
    param(
        [Parameter( Mandatory, ParameterSetName = 'byId' )]
        [Int[]]
        $Id,

        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'byInputVersion' )]
        [PSTypeName('JiraPS.Version')]
        $InputVersion,

        [Parameter( Position = 0, Mandatory , ParameterSetName = 'byProject' )]
        [Alias('Key')]
        [String[]]
        $Project,

        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'byInputProject' )]
        [PSTypeName('JiraPS.Project')]
        $InputProject,

        [Parameter( ParameterSetName = 'byProject' )]
        [Parameter( ParameterSetName = 'byInputProject' )]
        [Alias('Versions')]
        [String[]]
        $Name = "*",

        [Parameter( ParameterSetName = 'byProject')]
        [Parameter( ParameterSetName = 'byInputProject')]
        [ValidateSet("sequence",
            "name",
            "startDate",
            "releaseDate"
        )]
        [String]
        $Sort = "name",

        [UInt32]
        $PageSize = $script:DefaultPageSize,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $ParameterSetName = ''
        switch ($PsCmdlet.ParameterSetName) {
            'byInputProject' { $Project = $InputProject.Key; $ParameterSetName = 'byProject' }
            'byInputVersion' { $Id = $InputVersion.Id; $ParameterSetName = 'byId' }
            'byProject' { $ParameterSetName = 'byProject' }
            'byId' { $ParameterSetName = 'byId' }
        }

        switch ($ParameterSetName) {
            "byId" {
                foreach ($_id in $ID) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

                    $parameter = @{
                        URI        = $resourceURi -f "version/$_id"
                        Method     = "GET"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    $result = Invoke-JiraMethod @parameter

                    Write-Output (ConvertTo-JiraVersion -InputObject $result)
                }
            }
            "byProject" {
                foreach ($_project in $Project) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_project]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_project [$_project]"

                    $projectData = Get-JiraProject -Project $_project -Credential $Credential

                    $parameter = @{
                        URI          = $resourceURi -f "project/$($projectData.key)/version"
                        Method       = "GET"
                        GetParameter = @{
                            orderBy    = $Sort
                            maxResults = $PageSize
                        }
                        Paging       = $true
                        OutputType   = "JiraVersion"
                        Credential   = $Credential
                    }
                    # Paging
                    ($PSCmdlet.PagingParameters | Get-Member -MemberType Property).Name | ForEach-Object {
                        $parameter[$_] = $PSCmdlet.PagingParameters.$_
                    }

                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    if ($result = Invoke-JiraMethod @parameter) {
                        $result | Where-Object {
                            $__ = $_.Name
                            Write-DebugMessage ($__ | Out-String)
                            $Name | Foreach-Object {
                                Write-Verbose "[$($MyInvocation.MyCommand.Name)] Matching $_ against $($__)"
                                $__ -like $_
                            }
                        }
                    }
                }
            }
        }
    }
    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Invoke-JiraIssueTransition {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter( Mandatory )]
        [Object]
        $Transition,

        [PSCustomObject]
        $Fields,

        [Object]
        $Assignee,

        [String]
        $Comment,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Passthru
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        if ("JiraPS.Transition" -in $Transition.PSObject.TypeNames) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Transition parameter is a JiraPS.Transition object"
            $transitionId = $Transition.Id
        }
        else {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Attempting to cast Transition parameter [$Transition] as int for transition ID"
            try {
                $transitionId = [Int]"$Transition"
            }
            catch {
                $exception = ([System.ArgumentException]"Invalid Type for Parameter")
                $errorId = 'ParameterType.NotJiraTransition'
                $errorCategory = 'InvalidArgumenty'
                $errorTarget = $Transition
                $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTargetError
                $errorItem.ErrorDetails = "Wrong object type provided for Transition. Expected [JiraPS.Transition] or [Int], but was $($Transition.GetType().Name)"
                $PSCmdlet.ThrowTerminatingError($errorItem)
            }
        }

        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Checking that the issue can perform the given transition"
        if (($issueObj.Transition.Id) -notcontains $transitionId) {
            $exception = ([System.ArgumentException]"Invalid value for Parameter")
            $errorId = 'ParameterValue.InvalidTransition'
            $errorCategory = 'InvalidArgument'
            $errorTarget = $Issue
            $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
            $errorItem.ErrorDetails = "The specified Jira issue cannot perform transition [$transitionId]. Check the issue's Transition property and provide a transition valid for its current state."
            $PSCmdlet.ThrowTerminatingError($errorItem)
        }

        $requestBody = @{
            'transition' = @{
                'id' = $transitionId
            }
        }

        if ($Assignee) {
            if ($Assignee -eq 'Unassigned') {
                <#
                  #ToDo:Deprecated
                  This behavior should be deprecated
                #>
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] 'Unassigned' String passed. Issue will be assigned to no one."
                $assigneeString = ""
                $validAssignee = $true
            }
            else {
                if ($assigneeObj = Resolve-JiraUser -InputObject $Assignee -Credential $Credential -Exact) {
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] User found (name=[$($assigneeObj.Name)],RestUrl=[$($assigneeObj.RestUrl)])"
                    $assigneeString = $assigneeObj.Name
                    $validAssignee = $true
                }
                else {
                    $exception = ([System.ArgumentException]"Invalid value for Parameter")
                    $errorId = 'ParameterValue.InvalidAssignee'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $Assignee
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Unable to validate Jira user [$Assignee]. Use Get-JiraUser for more details."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
            }
        }

        if ($validAssignee) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Updating Assignee"
            $requestBody += @{
                'fields' = @{
                    'assignee' = @{
                        'name' = $assigneeString
                    }
                }
            }
        }

        $requestBody += @{
            'update' = @{}
        }

        if ($Fields) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Resolving `$Fields"
            foreach ($key in $Fields.Keys) {
                $name = $key
                $value = $Fields.$key
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Attempting to identify field (name=[$name], value=[$value])"

                if ($field = Get-JiraField -Field $name -Credential $Credential) {
                    # For some reason, this was coming through as a hashtable instead of a String,
                    # which was causing ConvertTo-Json to crash later.
                    # Not sure why, but this forces $id to be a String and not a hashtable.
                    $id = "$($field.ID)"
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Field [$name] was identified as ID [$id]"
                    $requestBody.update.$id = @( @{
                            'set' = $value
                        })
                }
                else {
                    $exception = ([System.ArgumentException]"Invalid value for Parameter")
                    $errorId = 'ParameterValue.InvalidFields'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $Fields
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Unable to identify field [$name] from -Fields hashtable. Use Get-JiraField for more information."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
            }
        }

        if ($Comment) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding comment"
            $requestBody.update.comment += , @{
                'add' = @{
                    'body' = $Comment
                }
            }
        }

        $parameter = @{
            URI        = "{0}/transitions" -f $issueObj.RestURL
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody -Depth 4
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        Invoke-JiraMethod @parameter

        if ($Passthru) {
            Get-JiraIssue $issueObj
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Invoke-JiraMethod {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsPaging )]
    param(
        [Parameter( Mandatory )]
        [Uri]
        $URI,

        [Microsoft.PowerShell.Commands.WebRequestMethod]
        $Method = "GET",

        [String]
        $Body,

        [Switch]
        $RawBody,

        [Hashtable]
        $Headers = @{},

        [Hashtable]
        $GetParameter = @{},

        [Switch]
        $Paging,

        [String]
        $InFile,

        [String]
        $OutFile,

        [Switch]
        $StoreSession,

        [ValidateSet(
            "JiraComment",
            "JiraIssue",
            "JiraUser",
            "JiraVersion"
        )]
        [String]
        $OutputType,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        # [Parameter( DontShow )]
        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $PSCmdlet
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        Set-TlsLevel -Tls12

        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # load DefaultParameters for Invoke-WebRequest
        # as the global PSDefaultParameterValues is not used
        $PSDefaultParameterValues = Resolve-DefaultParameterValue -Reference $global:PSDefaultParameterValues -CommandName 'Invoke-WebRequest'

        #region Headers
        # Construct the Headers with the folling priority:
        # - Headers passes as parameters
        # - User's Headers in $PSDefaultParameterValues
        # - Module's default Headers
        $_headers = Join-Hashtable -Hashtable $script:DefaultHeaders, $PSDefaultParameterValues["Invoke-WebRequest:Headers"], $Headers
        #endregion Headers

        #region Manage URI
        # Amend query from URI with GetParameter
        $uriQuery = ConvertTo-ParameterHash -Uri $Uri
        $internalGetParameter = Join-Hashtable $uriQuery, $GetParameter

        # And remove it from URI
        [Uri]$Uri = $Uri.GetLeftPart("Path")
        $PaginatedUri = $Uri

        # Use default PageSize
        if (-not $internalGetParameter.ContainsKey("maxResults")) {
            $internalGetParameter["maxResults"] = $script:DefaultPageSize
        }

        # Append GET parameters to URi
        $offset = 0
        if ($PSCmdlet.PagingParameters) {
            if ($PSCmdlet.PagingParameters.Skip) {
                $internalGetParameter["startAt"] = $PSCmdlet.PagingParameters.Skip
                $offset = $PSCmdlet.PagingParameters.Skip
            }
            if ($PSCmdlet.PagingParameters.First -lt $internalGetParameter["maxResults"]) {
                $internalGetParameter["maxResults"] = $PSCmdlet.PagingParameters.First
            }
        }

        [Uri]$PaginatedUri = "{0}{1}" -f $PaginatedUri, (ConvertTo-GetParameter $internalGetParameter)
        #endregion Manage URI

        #region Constructe IWR Parameter
        $splatParameters = @{
            Uri             = $PaginatedUri
            Method          = $Method
            Headers         = $_headers
            ContentType     = $script:DefaultContentType
            UseBasicParsing = $true
            Credential      = $Credential
            ErrorAction     = "Stop"
            Verbose         = $false
        }

        if ($_headers.ContainsKey("Content-Type")) {
            $splatParameters["ContentType"] = $_headers["Content-Type"]
            $splatParameters["Headers"].Remove("Content-Type")
            $_headers.Remove("Content-Type")
        }

        if ($Body) {
            if ($RawBody) {
                $splatParameters["Body"] = $Body
            }
            else {
                # Encode Body to preserve special chars
                # http://stackoverflow.com/questions/15290185/invoke-webrequest-issue-with-special-characters-in-json
                $splatParameters["Body"] = [System.Text.Encoding]::UTF8.GetBytes($Body)
            }
        }

        if ((-not $Credential) -or ($Credential -eq [System.Management.Automation.PSCredential]::Empty)) {
            $splatParameters.Remove("Credential")
            if ($session = Get-JiraSession -ErrorAction SilentlyContinue) {
                $splatParameters["WebSession"] = $session.WebSession
            }
        }

        if ($StoreSession) {
            $splatParameters["SessionVariable"] = "newSessionVar"
            $splatParameters.Remove("WebSession")
        }

        if ($InFile) {
            $splatParameters["InFile"] = $InFile
        }
        if ($OutFile) {
            $splatParameters["OutFile"] = $OutFile
        }
        #endregion Constructe IWR Parameter

        #region Execute the actual query
        # Normal ProgressPreference really slows down invoke-webrequest as it tries to update the screen for bytes received.
        # By setting ProgressPreference to silentlyContinue it doesn't try to update the screen and speeds up the downloads.
        # See https://stackoverflow.com/a/43477248/2641196
        $oldProgressPreference = $progressPreference
        $progressPreference = 'silentlyContinue'
        try {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] $($splatParameters.Method) $($splatParameters.Uri)"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoke-WebRequest with `$splatParameters: $($splatParameters | Out-String)"
            # Invoke the API
            $webResponse = Invoke-WebRequest @splatParameters
        }
        catch {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Failed to get an answer from the server"

            $exception = $_
            $webResponse = $exception.Exception.Response
        }
        # Reset the progressPreference to the value it was before the Invoke-WebRequest
        $progressPreference = $oldProgressPreference

        Write-Debug "[$($MyInvocation.MyCommand.Name)] Executed WebRequest. Access `$webResponse to see details"
        Test-ServerResponse -InputObject $webResponse -Cmdlet $Cmdlet
        #endregion Execute the actual query
    }

    process {
        if ($webResponse) {
            # In PowerShellCore (v6+) the StatusCode of an exception is somewhere else
            if (-not ($statusCode = $webResponse.StatusCode)) {
                $statusCode = $webResponse.Exception.Response.StatusCode
            }
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Status code: $($statusCode)"

            #region Code 400+
            if ($statusCode.value__ -ge 400) {
                Resolve-ErrorWebResponse -Exception $exception -StatusCode $statusCode -Cmdlet $Cmdlet
            }
            #endregion Code 400+

            #region Code 399-
            else {
                if ($StoreSession) {
                    return & $script:SessionTransformationMethod -Session $newSessionVar -Username $Credential.UserName
                }

                if ($webResponse.Content) {
                    $response = ConvertFrom-Json ([Text.Encoding]::UTF8.GetString($webResponse.RawContentStream.ToArray()))

                    if ($Paging) {
                        # Remove Parameters that don't need propagation
                        $script:PSDefaultParameterValues.Remove("$($MyInvocation.MyCommand.Name):IncludeTotalCount")
                        $null = $PSBoundParameters.Remove("Paging")
                        $null = $PSBoundParameters.Remove("Skip")
                        if (-not $PSBoundParameters["GetParameter"]) {
                            $PSBoundParameters["GetParameter"] = $internalGetParameter
                        }

                        $total = 0
                        do {
                            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Invoking pagination [currentTotal: $total]"

                            $result = Expand-Result -InputObject $response

                            $total += @($result).Count
                            $pageSize = $response.maxResults

                            if ($total -gt $PSCmdlet.PagingParameters.First) {
                                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Only output the first $($PSCmdlet.PagingParameters.First % $pageSize) of page"
                                $result = $result | Select-Object -First ($PSCmdlet.PagingParameters.First % $pageSize)
                            }

                            Convert-Result -InputObject $result -OutputType $OutputType
                            Write-DebugMessage ($result | Out-String)

                            if (@($result).Count -lt $response.maxResults) {
                                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Stopping paging, as page had less entries than $($response.maxResults)"
                                break
                            }

                            if ($total -ge $PSCmdlet.PagingParameters.First) {
                                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Stopping paging, as $total reached $($PSCmdlet.PagingParameters.First)"
                                break
                            }

                            # calculate the size of the next page
                            $PSBoundParameters["GetParameter"]["startAt"] = $total + $offset
                            $expectedTotal = $PSBoundParameters["GetParameter"]["startAt"] + $pageSize
                            if ($expectedTotal -gt $PSCmdlet.PagingParameters.First) {
                                $reduceBy = $expectedTotal - $PSCmdlet.PagingParameters.First
                                $PSBoundParameters["GetParameter"]["maxResults"] = $pageSize - $reduceBy
                            }

                            # Inquire the next page
                            $response = Invoke-JiraMethod @PSBoundParameters

                            $result = Expand-Result -InputObject $response
                        } while (@($result).Count -gt 0)

                        if ($PSCmdlet.PagingParameters.IncludeTotalCount) {
                            [double]$Accuracy = 1.0
                            $PSCmdlet.PagingParameters.NewTotalCount($total, $Accuracy)
                        }
                    }
                    else {
                        $response
                    }
                }
                else {
                    # No content, although statusCode < 400
                    # This could be wanted behavior of the API
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] No content was returned from."
                }
            }
            #endregion Code 399-
        }
        else {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] No Web result object was returned from. This is unusual!"
        }
    }

    end {
        Set-TlsLevel -Revert

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function ended"
    }
}

function Move-JiraVersion {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( DefaultParameterSetName = 'ByAfter' )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Version" -notin $_.PSObject.TypeNames) -and (($_ -isnot [Int]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraVersion'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Version. Expected [JiraPS.Version] or [Int], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $Version,

        [Parameter( Mandatory, ParameterSetName = 'ByPosition' )]
        [ValidateSet('First', 'Last', 'Earlier', 'Later')]
        [String]$Position,

        [Parameter( Mandatory, ParameterSetName = 'ByAfter' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Version" -notin $_.PSObject.TypeNames) -and (($_ -isnot [Int]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraVersion'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Version. Expected [JiraPS.Version] or [Int], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $After,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $versionResourceUri = "$server/rest/api/2/version/{0}/move"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $requestBody = @{ }
        switch ($PsCmdlet.ParameterSetName) {
            'ByPosition' {
                $requestBody["position"] = $Position
            }
            'ByAfter' {
                $afterSelfUri = ''
                if ($After -is [Int]) {
                    $versionObj = Get-JiraVersion -Id $After -Credential $Credential -ErrorAction Stop
                    $afterSelfUri = $versionObj.RestUrl
                }
                else {
                    $afterSelfUri = $After.RestUrl
                }

                $requestBody["after"] = $afterSelfUri
            }
        }

        if ($Version.Id) {
            $versionId = $Version.Id
        } else {
            $versionId = $Version
        }

        $parameter = @{
            URI        = $versionResourceUri -f $versionId
            Method     = "POST"
            Body       = ConvertTo-Json $requestBody
            Credential = $Credential
        }

        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        Invoke-JiraMethod @parameter
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function New-JiraFilter {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [String]
        $Name,

        [Parameter( ValueFromPipelineByPropertyName )]
        [String]
        $Description,

        [Parameter( Mandatory, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [String]
        $JQL,

        [Parameter( ValueFromPipelineByPropertyName )]
        [Alias('Favourite')]
        [Switch]
        $Favorite,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/filter"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $requestBody = @{
            name = $Name
            jql  = $JQL
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Description")) {
            $requestBody["description"] = $Description
        }
        $requestBody["favourite"] = [Bool]$Favorite

        $parameter = @{
            URI        = $resourceURi
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($Name, "Creating new Filter")) {
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraFilter -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function New-JiraGroup {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [Alias('Name')]
        [String[]]
        $GroupName,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_group in $GroupName) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_group [$_group]"

            $requestBody = @{
                "name" = $_group
            }

            $parameter = @{
                URI        = $resourceURi
                Method     = "POST"
                Body       = ConvertTo-Json -InputObject $requestBody
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($GroupName, "Creating group [$GroupName] to JIRA")) {
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraGroup -InputObject $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function New-JiraIssue {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipelineByPropertyName )]
        [String]
        $Project,

        [Parameter( Mandatory, ValueFromPipelineByPropertyName )]
        [String]
        $IssueType,

        [Parameter( Mandatory, ValueFromPipelineByPropertyName )]
        [String]
        $Summary,

        [Parameter( ValueFromPipelineByPropertyName )]
        [Int]
        $Priority,

        [Parameter( ValueFromPipelineByPropertyName )]
        [String]
        $Description,

        [Parameter( ValueFromPipelineByPropertyName )]
        [AllowNull()]
        [AllowEmptyString()]
        [String]
        $Reporter,

        [Parameter( ValueFromPipelineByPropertyName )]
        [String[]]
        $Labels,

        [Parameter( ValueFromPipelineByPropertyName )]
        [String]
        $Parent,

        [Parameter( ValueFromPipelineByPropertyName )]
        [Alias('FixVersions')]
        [String[]]
        $FixVersion,

        [Parameter( ValueFromPipelineByPropertyName )]
        [PSCustomObject]
        $Fields,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        $server = Get-JiraConfigServer -ErrorAction Stop -Debug:$false

        $createmeta = Get-JiraIssueCreateMetadata -Project $Project -IssueType $IssueType -Credential $Credential -ErrorAction Stop -Debug:$false

        $resourceURi = "$server/rest/api/2/issue"

        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $ProjectObj = Get-JiraProject -Project $Project -Credential $Credential -ErrorAction Stop -Debug:$false
        $issueTypeObj = $projectObj.IssueTypes | Where-Object -FilterScript {$_.Id -eq $IssueType -or $_.Name -eq $IssueType}

        if ($null -eq $issueTypeObj.Id)
        {
            $errorMessage = @{
                Category         = "InvalidResult"
                CategoryActivity = "Validating parameters"
                Message          = "No issue types were found in the project [$Project] for the given issue type [$IssueType]. Use Get-JiraIssueType for more details."
            }
            Write-Error @errorMessage
        }

        $requestBody = @{
            "project"   = @{"id" = $ProjectObj.Id}
            "issuetype" = @{"id" = [String] $IssueTypeObj.Id}
            "summary"   = $Summary
        }

        if ($Priority) {
            $requestBody["priority"] = @{"id" = [String] $Priority}
        }

        if ($Description) {
            $requestBody["description"] = $Description
        }

        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Reporter")) {
            $requestBody["reporter"] = @{"name" = "$Reporter"}
        }
        elseif ($ProjectObj.Style -eq "next-gen"){
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding reporter as next-gen projects must have reporter set."
            $requestBody["reporter"] = @{"name" = "$((Get-JiraUser -Credential $Credential).Name)"}
        }

        if ($Parent) {
            $requestBody["parent"] = @{"key" = $Parent}
        }

        if ($Labels) {
            $requestBody["labels"] = [System.Collections.ArrayList]@()
            foreach ($item in $Labels) {
                $null = $requestBody["labels"].Add($item)
            }
        }

        if ($FixVersion) {
            $requestBody['fixVersions'] = [System.Collections.ArrayList]@()
            foreach ($item in $FixVersion) {
                $null = $requestBody["fixVersions"].Add( @{ name = "$item" } )
            }
        }

        Write-Debug "[$($MyInvocation.MyCommand.Name)] Resolving `$Fields"
        foreach ($_key in $Fields.Keys) {
            $name = $_key
            $value = $Fields.$_key

            if ($field = Get-JiraField -Field $name -Credential $Credential -Debug:$false) {
                # For some reason, this was coming through as a hashtable instead of a String,
                # which was causing ConvertTo-Json to crash later.
                # Not sure why, but this forces $id to be a String and not a hashtable.
                $id = $field.Id
                $requestBody["$id"] = $value
            }
            else {
                $exception = ([System.ArgumentException]"Invalid value for Parameter")
                $errorId = 'ParameterValue.InvalidFields'
                $errorCategory = 'InvalidArgument'
                $errorTarget = $Fields
                $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                $errorItem.ErrorDetails = "Unable to identify field [$name] from -Fields hashtable. Use Get-JiraField for more information."
                $PSCmdlet.ThrowTerminatingError($errorItem)
            }
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Validating fields with metadata"
        foreach ($c in $createmeta) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Checking metadata for `$c [$c]"
            if ($c.Required) {
                if ($requestBody.ContainsKey($c.Id)) {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Required field (id=[$($c.Id)], name=[$($c.Name)]) was provided (value=[$($requestBody.$($c.Id))])"
                }
                else {
                    $exception = ([System.ArgumentException]"Invalid or missing value Parameter")
                    $errorId = 'ParameterValue.CreateMetaFailure'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $Fields
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Jira's metadata for project [$Project] and issue type [$IssueType] specifies that a field is required that was not provided (name=[$($c.Name)], id=[$($c.Id)]). Use Get-JiraIssueCreateMetadata for more information."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
            }
            else {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Non-required field (id=[$($c.Id)], name=[$($c.Name)])"
            }
        }

        $hashtable = @{
            'fields' = ([PSCustomObject]$requestBody)
        }

        $parameter = @{
            URI        = $resourceURi
            Method     = "POST"
            Body       = (ConvertTo-Json -InputObject ([PSCustomObject]$hashtable) -Depth 7)
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($Summary, "Creating new Issue on JIRA")) {
            if ($result = Invoke-JiraMethod @parameter) {
                # REST result will look something like this:
                # {"id":"12345","key":"IT-3676","self":"http://jiraserver.example.com/rest/api/2/issue/12345"}
                # This will fetch the created issue to return it with all it'a properties
                Write-Output (Get-JiraIssue -Key $result.Key -Credential $Credential)
            }
        }
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }

    end {
    }
}

function New-JiraSession {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseShouldProcessForStateChangingFunctions', '')]
    param(
        [Parameter( Mandatory )]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential,

        [Hashtable]
        $Headers = @{ }
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/myself"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $parameter = @{
            URI          = $resourceURi
            Method       = "GET"
            Headers      = $Headers
            StoreSession = $true
            Credential   = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        $result = Invoke-JiraMethod @parameter

        if ($MyInvocation.MyCommand.Module.PrivateData) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding session result to existing module PrivateData"
            $MyInvocation.MyCommand.Module.PrivateData.Session = $result
        }
        else {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Creating module PrivateData"
            $MyInvocation.MyCommand.Module.PrivateData = @{
                'Session' = $result
            }
        }

        Write-Output $result
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function New-JiraUser {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [String]
        $UserName,

        [Parameter( Mandatory )]
        [Alias('Email')]
        [String]
        $EmailAddress,

        [String]
        $DisplayName,

        [Boolean]
        $Notify = $true,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/user"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $requestBody = @{
            "name"         = $UserName
            "emailAddress" = $EmailAddress
            "notification" = $Notify
        }

        if ($DisplayName) {
            $requestBody.displayName = $DisplayName
        }
        else {
            Write-DebugMessage "[New-JiraUser] DisplayName was not specified; defaulting to UserName parameter [$UserName]"
            $requestBody.displayName = $UserName
        }

        $parameter = @{
            URI        = $resourceURi
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($UserName, "Creating new User on JIRA")) {
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraUser -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function New-JiraVersion {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, DefaultParameterSetName = 'byObject' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'byObject' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Version" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraVersion'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Version. Expected [JiraPS.Version] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $InputObject,

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'byParameters' )]
        [String]
        $Name,

        [Parameter( Position = 1, Mandatory, ParameterSetName = 'byParameters' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                $Input = $_

                switch ($true) {
                    {"JiraPS.Project" -in $Input.PSObject.TypeNames} { return $true }
                    {$Input -is [String]} { return $true}
                    Default {
                        $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                        $errorId = 'ParameterType.NotJiraProject'
                        $errorCategory = 'InvalidArgument'
                        $errorTarget = $Input
                        $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                        $errorItem.ErrorDetails = "Wrong object type provided for Project. Expected [JiraPS.Project] or [String], but was $($Input.GetType().Name)"
                        $PSCmdlet.ThrowTerminatingError($errorItem)
                        <#
                          #ToDo:CustomClass
                          Once we have custom classes, this check can be done with Type declaration
                        #>
                    }
                }
            }
        )]
        [Object]
        $Project,

        [Parameter( ParameterSetName = 'byParameters' )]
        [String]
        $Description,

        [Parameter( ParameterSetName = 'byParameters' )]
        [Bool]
        $Archived,

        [Parameter( ParameterSetName = 'byParameters' )]
        [Bool]
        $Released,

        [Parameter( ParameterSetName = 'byParameters' )]
        [DateTime]
        $ReleaseDate,

        [Parameter( ParameterSetName = 'byParameters' )]
        [DateTime]
        $StartDate,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/version"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $requestBody = @{}
        Switch ($PSCmdlet.ParameterSetName) {
            'byObject' {
                $requestBody["name"] = $InputObject.Name
                $requestBody["description"] = $InputObject.Description
                $requestBody["archived"] = [bool]($InputObject.Archived)
                $requestBody["released"] = [bool]($InputObject.Released)
                $requestBody["releaseDate"] = $InputObject.ReleaseDate.ToString('yyyy-MM-dd')
                $requestBody["startDate"] = $InputObject.StartDate.ToString('yyyy-MM-dd')
                if ($InputObject.Project.Key) {
                    $requestBody["project"] = $InputObject.Project.Key
                }
                elseif ($InputObject.Project.Id) {
                    $requestBody["projectId"] = $InputObject.Project.Id
                }
            }
            'byParameters' {
                $requestBody["name"] = $Name
                if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Description")) {
                    $requestBody["description"] = $Description
                }
                if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Archived")) {
                    $requestBody["archived"] = $Archived
                }
                if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Released")) {
                    $requestBody["released"] = $Released
                }
                if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("ReleaseDate")) {
                    $requestBody["releaseDate"] = Get-Date $ReleaseDate -Format 'yyyy-MM-dd'
                }
                if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("StartDate")) {
                    $requestBody["startDate"] = Get-Date $StartDate -Format 'yyyy-MM-dd'
                }

                if ("JiraPS.Project" -in $Project.PSObject.TypeNames) {
                    if ($Project.Id) {
                        $requestBody["projectId"] = $Project.Id
                    }
                    elseif ($Project.Key) {
                        $requestBody["project"] = $Project.Key
                    }
                }
                else {
                    $requestBody["projectId"] = (Get-JiraProject $Project -Credential $Credential).Id
                }
            }
        }

        $parameter = @{
            URI        = $resourceURi
            Method     = "POST"
            Body       = ConvertTo-Json -InputObject $requestBody
            Credential = $Credential
        }
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
        if ($PSCmdlet.ShouldProcess($Name, "Creating new Version on JIRA")) {
            $result = Invoke-JiraMethod @parameter

            Write-Output (ConvertTo-JiraVersion -InputObject $result)
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraFilter {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( ConfirmImpact = "Medium", SupportsShouldProcess, DefaultParameterSetName = 'ByInputObject' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ByInputObject' )]
        [ValidateNotNullOrEmpty()]
        [PSTypeName('JiraPS.Filter')]
        $InputObject,

        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ById')]
        [ValidateNotNullOrEmpty()]
        [UInt32[]]
        $Id,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if ($PSCmdlet.ParameterSetName -eq 'ById') {
            $InputObject = foreach ($_id in $Id) {
                Get-JiraFilter -Id $_id
            }
        }

        foreach ($filter in $InputObject) {
            $parameter = @{
                URI        = $filter.RestURL
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($filter.Name, "Deleting Filter")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraFilterPermission {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, DefaultParameterSetName = 'ByFilterId' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ParameterSetName = 'ByFilterObject' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (@($Filter).Count -gt 1) {
                    $exception = ([System.ArgumentException]"Invalid Parameter")
                    $errorId = 'ParameterType.TooManyFilters'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Only one Filter can be passed at a time."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                elseif (@($_.FilterPermissions).Count -lt 1) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter")
                    $errorId = 'ParameterType.FilterWithoutPermission'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "The Filter provided does not contain any Permission to delete."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [PSTypeName('JiraPS.Filter')]
        $Filter,

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByFilterId' )]
        [ValidateNotNullOrEmpty()]
        [Alias('Id')]
        [UInt32]
        $FilterId,

        # TODO: [Parameter( Position = 1, ParameterSetName = 'ByFilterObject')]
        [Parameter( Position = 1, Mandatory, ParameterSetName = 'ByFilterId')]
        [ValidateNotNullOrEmpty()]
        [UInt32[]]
        $PermissionId,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PSCmdlet.ParameterSetName) {
            "ByFilterObject" {
                $PermissionId = $Filter.FilterPermissions.Id
            }
            "ByFilterId" {
                $Filter = Get-JiraFilter -Id $FilterId
            }
        }

        foreach ($_permissionId in $PermissionId) {
            $parameter = @{
                URI        = "{0}/permission/{1}" -f $Filter.RestURL, $_permissionId
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($InputObject.Type, "Remove Permission")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraGroup {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, ConfirmImpact = 'High' )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Group" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraGroup'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Group. Expected [JiraPS.Group] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('GroupName')]
        [Object[]]
        $Group,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group?groupname={0}"

        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_group in $Group) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_group [$_group]"

            $groupObj = Get-JiraGroup -GroupName $_group -Credential $Credential -ErrorAction Stop

            $parameter = @{
                URI        = $resourceURi -f $groupObj.Name
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($groupObj.Name, "Remove group")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraGroupMember {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, ConfirmImpact = 'High' )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Group" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraGroup'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Group. Expected [JiraPS.Group] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('GroupName')]
        [Object[]]
        $Group,

        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.User" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.UotJirauser'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for User. Expected [JiraPS.User] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('UserName')]
        [Object[]]
        $User,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/group/user?groupname={0}&username={1}"

        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_group in $Group) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_group]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_group [$_group]"

            $groupObj = Get-JiraGroup -GroupName $_group -Credential $Credential -ErrorAction Stop
            # $groupMembers = (Get-JiraGroupMember -Group $_group -Credential $Credential -ErrorAction Stop).Name

            foreach ($_user in $User) {
                Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_user]"
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_user [$_user]"

                $userObj = Resolve-JiraUser -InputObject $_user -Exact -Credential $Credential -ErrorAction Stop

                # if ($groupMembers -contains $userObj.Name) {
                # TODO: test what jira says
                $parameter = @{
                    URI        = $resourceURi -f $groupObj.Name, $userObj.Name
                    Method     = "DELETE"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                if ($PSCmdlet.ShouldProcess($groupObj.Name, "Remove $($userObj.Name) from group")) {
                    Invoke-JiraMethod @parameter
                }
                # }
            }

            if ($PassThru) {
                Write-Output (Get-JiraGroup -InputObject $groupObj -Credential $Credential)
            }
        }
    }

    end {
        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraIssue {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding(
        ConfirmImpact = 'High',
        SupportsShouldProcess,
        DefaultParameterSetName = "ByInputObject"
    )]
    param (
        [Parameter(
            Mandatory,
            ValueFromPipeline,
            Position = 0,
            ParameterSetName = "ByInputObject"
        )]
        [Alias(
            "Issue"
        )]
        [PSTypeName("JiraPS.Issue")]
        [Object[]]
        $InputObject,

        # The issue's ID number or key.
        [Parameter(
            Mandatory,
            Position = 0,
            ParameterSetName = "ByIssueId"
        )]
        [ValidateNotNullOrEmpty()]
        [Alias(
            "Id",
            "Key",
            "issueIdOrKey"
        )]
        [String[]]
        $IssueId,

        [Switch]
        [Alias("deleteSubtasks")]
        $IncludeSubTasks,

        [System.Management.Automation.CredentialAttribute()]
        [System.Management.Automation.PSCredential]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issue/{0}?deleteSubtasks={1}"

        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {

        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PsCmdlet.ParameterSetName) {
            "ByInputObject" { $PrimaryIterator = $InputObject }
            "ByIssueId" { $PrimaryIterator = $IssueID }
        }

        foreach ($issueItem in $PrimaryIterator) {

            if ($PsCmdlet.ParameterSetName -eq "ByIssueId") {
                $_issue = Get-JiraIssue -Key $issueItem -Credential $Credential -ErrorAction Stop
            } Else {
                $_issue = $issueItem
            }

            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issue]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$issueItem [$_issue]"



            $parameter = @{
                URI        = $resourceURi -f $_issue.Key,$IncludeSubTasks
                Method     = "DELETE"
                Credential = $Credential
                Cmdlet = $PsCmdlet
            }


            If ($IncludeSubTasks) {
                $ActionText = "Remove issue and sub-tasks"
            } Else {
                $ActionText = "Remove issue"
            }

            if ($PSCmdlet.ShouldProcess($_issue.ToString(), $ActionText)) {

                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                Invoke-JiraMethod @parameter
            }
        }

    }

    end {
        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraIssueAttachment {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( ConfirmImpact = 'High', SupportsShouldProcess, DefaultParameterSetName = 'byId' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipelineByPropertyName, ParameterSetName = 'byId' )]
        [ValidateNotNullOrEmpty()]
        [Alias('Id')]
        [Int[]]
        $AttachmentId,

        [Parameter( Position = 0, Mandatory, ParameterSetName = 'byIssue' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter( ParameterSetName = 'byIssue' )]
        [String[]]
        $FileName,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/attachment/{0}"

        if ($Force) {
            Write-DebugMessage "[Remove-JiraGroupMember] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        switch ($PsCmdlet.ParameterSetName) {
            "byId" {
                foreach ($_id in $AttachmentId) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_id]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_id [$_id]"

                    $parameter = @{
                        URI        = $resourceURi -f $_id
                        Method     = "DELETE"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    if ($PSCmdlet.ShouldProcess($thisUrl, "Removing an attachment")) {
                        Invoke-JiraMethod @parameter
                    }
                }
            }
            "byIssue" {
                Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$Issue]"
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$Issue [$Issue]"

                if (@($Issue).Count -ne 1) {
                    $exception = ([System.ArgumentException]"invalid Issue provided")
                    $errorId = 'ParameterValue.JiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Only one Issue can be provided at a time."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }

                # Find the proper object for the Issue
                $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential
                $attachments = Get-JiraIssueAttachment -Issue $IssueObj -Credential $Credential -ErrorAction Stop

                if ($FileName) {
                    $_attachments = @()
                    foreach ($file in $FileName) {
                        $_attachments += $attachments | Where-Object {$_.FileName -like $file}
                    }
                    $attachments = $_attachments
                }

                foreach ($attachment in $attachments) {
                    Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$attachment]"
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$attachment [$attachment]"

                    $parameter = @{
                        URI        = $resourceURi -f $attachment.Id
                        Method     = "DELETE"
                        Credential = $Credential
                    }
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                    if ($PSCmdlet.ShouldProcess($Issue.Key, "Removing attachment '$($attachment.FileName)'")) {
                        Invoke-JiraMethod @parameter
                    }
                }
            }
        }
    }

    end {
        if ($Force) {
            Write-DebugMessage "[Remove-JiraGroupMember] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraIssueLink {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, ConfirmImpact = 'Medium' )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                $Input = $_
                $objectProperties = $Input | Get-Member -MemberType *Property
                switch ($true) {
                    {("JiraPS.Issue" -in $Input.PSObject.TypeNames) -and ("issueLinks" -in $objectProperties.Name)} { return $true }
                    {("JiraPS.IssueLink" -in $Input.PSObject.TypeNames) -and ("Id" -in $objectProperties.Name)} { return $true }
                    default {
                        $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                        $errorId = 'ParameterType.NotJiraIssue'
                        $errorCategory = 'InvalidArgument'
                        $errorTarget = $Input
                        $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                        $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue], [JiraPS.IssueLink] or [String], but was $($Input.GetType().Name)"
                        $PSCmdlet.ThrowTerminatingError($errorItem)
                        <#
                          #ToDo:CustomClass
                          Once we have custom classes, this check can be done with Type declaration
                        #>
                    }
                }
            }
        )]
        [Object[]]
        $IssueLink,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issueLink/{0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # As we are not able to use proper type casting in the parameters, this is a workaround
        # to extract the data from a JiraPS.Issue object
        <#
          #ToDo:CustomClass
          Once we have custom classes, this will no longer be necessary
        #>
        if ($IssueLink.issueLinks) {
            $IssueLink = $IssueLink.issueLinks
        }

        foreach ($link in $IssueLink) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$link]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$link [$link]"

            $parameter = @{
                URI        = $resourceURi -f $link.id
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($link.id, "Remove IssueLink")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraIssueWatcher {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory )]
        [string[]]
        $Watcher,

        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object]
        $Issue,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Find the proper object for the Issue
        $issueObj = Resolve-JiraIssueObject -InputObject $Issue -Credential $Credential

        foreach ($username in $Watcher) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$username]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$username [$username]"

            $parameter = @{
                URI        = "{0}/watchers?username={1}" -f $issueObj.RestURL, $username
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($IssueObj.Key, "Removing watcher '$($username)'")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraRemoteLink {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( ConfirmImpact = 'High', SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias("Key")]
        [Object[]]
        $Issue,

        [Parameter( Mandatory )]
        [Int[]]
        $LinkId,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/issue/{0}/remotelink/{1}"

        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_issue in $Issue) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$Issue]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$Issue [$Issue]"

            # Find the proper object for the Issue
            $issueObj = Resolve-JiraIssueObject -InputObject $_issue -Credential $Credential

            foreach ($_link in $LinkId) {
                Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_link]"
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_link [$_link]"

                $parameter = @{
                    URI        = $resourceURi -f $issueObj.Key, $_link
                    Method     = "DELETE"
                    Credential = $Credential
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                if ($PSCmdlet.ShouldProcess($issueObj.Key, "Remove RemoteLink '$_link'")) {
                    Invoke-JiraMethod @parameter
                }
            }
        }
    }

    end {
        if ($Force) {
            Write-DebugMessage "[Remove-JiraGroupMember] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraSession {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseShouldProcessForStateChangingFunctions', '')]
    param(
        [Parameter( ValueFromPipeline )]
        [Object]
        $Session
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        if ($Session = Get-JiraSession) {
            $MyInvocation.MyCommand.Module.PrivateData.Session = $null
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraUser {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( ConfirmImpact = 'High', SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.User" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraUser'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for User. Expected [JiraPS.User] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('UserName')]
        [Object[]]
        $User,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/user?username={0}"

        if ($Force) {
            Write-DebugMessage "[Remove-JiraGroup] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_user in $User) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_user]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_user [$_user]"

            $userObj = Resolve-JiraUser -InputObject $_user -Credential $Credential -ErrorAction Stop

            $parameter = @{
                URI        = $resourceURi -f $userObj.Name
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($userObj.Name, 'Remove user')) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        if ($Force) {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Remove-JiraVersion {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( ConfirmImpact = 'High', SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Version" -notin $_.PSObject.TypeNames) -and (($_ -isnot [Int]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraVersion'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Version. Expected [JiraPS.Version] or [Int], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $Version,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $Force
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        if ($Force) {
            Write-DebugMessage "[Remove-JiraVersion] -Force was passed. Backing up current ConfirmPreference [$ConfirmPreference] and setting to None"
            $oldConfirmPreference = $ConfirmPreference
            $ConfirmPreference = 'None'
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_version in $Version) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_version]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_version [$_version]"

            if ($_version.id) {
                $_version = $_version.Id
            }

            $versionObj = Get-JiraVersion -Id $_version -Credential $Credential -ErrorAction Stop

            $parameter = @{
                URI        = $versionObj.RestUrl
                Method     = "DELETE"
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($versionObj.Name, "Removing Version")) {
                Invoke-JiraMethod @parameter
            }
        }
    }

    end {
        if ($Force) {
            Write-Debug "[Remove-JiraVersion] Restoring ConfirmPreference to [$oldConfirmPreference]"
            $ConfirmPreference = $oldConfirmPreference
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraConfigServer {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseShouldProcessForStateChangingFunctions', '')]
    param(
        [Parameter( Mandatory )]
        [ValidateNotNullOrEmpty()]
        [Alias('Uri')]
        [Uri]
        $Server
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        $script:JiraServerUrl = $Server

        Set-Content -Value $Server -Path "$script:serverConfig"
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraFilter {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [PSTypeName('JiraPS.Filter')]
        $InputObject,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [String]
        $Name,

        [Parameter()]
        [String]
        $Description,

        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [String]
        $JQL,

        [Parameter()]
        [Alias('Favourite')]
        [Bool]
        $Favorite,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        $requestBody = @{}
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Name")) {
            $requestBody["name"] = $Name
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Description")) {
            $requestBody["description"] = $Description
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("JQL")) {
            $requestBody["jql"] = $JQL
        }
        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Favorite")) {
            $requestBody["favourite"] = $Favorite
        }

        if ($requestBody.Keys.Count) {
            $parameter = @{
                URI        = $InputObject.RestURL
                Method     = "PUT"
                Body       = ConvertTo-Json -InputObject $requestBody
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($InputObject.Name, "Update Filter")) {
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraFilter -InputObject $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraIssue {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object[]]
        $Issue,

        [String]
        $Summary,

        [String]
        $Description,

        [Alias('FixVersions')]
        [String[]]
        $FixVersion,

        [Object]
        $Assignee,

        [String[]]
        $Label,

        [PSCustomObject]
        $Fields,

        [String]
        $AddComment,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru,

        [Switch]
        $SkipNotification
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $fieldNames = $Fields.Keys
        if (-not ($Summary -or $Description -or $Assignee -or $Label -or $FixVersion -or $fieldNames -or $AddComment)) {
            $errorMessage = @{
                Category         = "InvalidArgument"
                CategoryActivity = "Validating Arguments"
                Message          = "The parameters provided do not change the Issue. No action will be performed"
            }
            Write-Error @errorMessage
            return
        }

        if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Assignee")) {
            if ($Assignee -eq 'Unassigned') {
                <#
                  #ToDo:Deprecated
                  This behavior should be deprecated
                #>
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] 'Unassigned' String passed. Issue will be assigned to no one."
                $assigneeString = $null
                $validAssignee = $true
            }
            elseif ($Assignee -eq "Default") {
                <#
                  #ToDo:Deprecated
                  This behavior should be deprecated
                #>
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] 'Default' String passed. Issue will be assigned to the default assignee."
                $assigneeString = "-1"
                $validAssignee = $true
            }
            else {
                if ($assigneeObj = Resolve-JiraUser -InputObject $Assignee -Exact -Credential $Credential) {
                    Write-Debug "[$($MyInvocation.MyCommand.Name)] User found (name=[$($assigneeObj.Name)],RestUrl=[$($assigneeObj.RestUrl)])"
                    $assigneeString = $assigneeObj.Name
                    $validAssignee = $true
                }
                else {
                    $exception = ([System.ArgumentException]"Invalid value for Parameter")
                    $errorId = 'ParameterValue.InvalidAssignee'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $Assignee
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Unable to validate Jira user [$Assignee]. Use Get-JiraUser for more details."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
            }
        }
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_issue in $Issue) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issue]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_issue [$_issue]"

            # Find the proper object for the Issue
            $issueObj = Resolve-JiraIssueObject -InputObject $_issue -Credential $Credential

            $issueProps = @{
                'update' = @{}
            }

            if ($Summary) {
                # Update properties need to be passed to JIRA as arrays
                $issueProps.update["summary"] = @(@{ 'set' = $Summary })
            }

            if ($Description) {
                $issueProps.update["description"] = @(@{ 'set' = $Description })
            }

            if ($FixVersion) {
                $fixVersionSet = [System.Collections.ArrayList]@()
                foreach ($item in $FixVersion) {
                    $null = $fixVersionSet.Add( @{ 'name' = $item } )
                }
                $issueProps.update["fixVersions"] = @( @{ set = $fixVersionSet } )
            }

            if ($AddComment) {
                $issueProps.update["comment"] = @(
                    @{
                        'add' = @{
                            'body' = $AddComment
                        }
                    }
                )
            }

            if ($Fields) {
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Resolving `$Fields"
                foreach ($_key in $Fields.Keys) {
                    $name = $_key
                    $value = $Fields.$_key

                    $field = Get-JiraField -Field $name -Credential $Credential -ErrorAction Stop

                    # For some reason, this was coming through as a hashtable instead of a String,
                    # which was causing ConvertTo-Json to crash later.
                    # Not sure why, but this forces $id to be a String and not a hashtable.
                    $id = [string]$field.Id
                    $issueProps.update[$id] = @(@{ 'set' = $value })
                }
            }

            if ($validAssignee) {
                $assigneeProps = @{
                    'name' = $assigneeString
                }
            }

            $SkipNotificationParams = @{}
            if ($SkipNotification) {
                Write-Verbose "[$($MyInvocation.MyCommand.Name)] Skipping notification for watchers"
                $SkipNotificationParams = @{notifyUsers = "false"}
            }

            if ( @($issueProps.update.Keys).Count -gt 0 ) {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Updating issue fields"

                $parameter = @{
                    URI          = $issueObj.RestUrl
                    Method       = "PUT"
                    Body         = ConvertTo-Json -InputObject $issueProps -Depth 10
                    Credential   = $Credential
                    GetParameter = $SkipNotificationParams
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                if ($PSCmdlet.ShouldProcess($issueObj.Key, "Updating Issue")) {
                    Invoke-JiraMethod @parameter
                }
            }

            if ($assigneeProps) {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Updating issue assignee"
                # Jira handles assignee differently; you can't change it from the default "edit issues" screen unless
                # you customize the "Edit Issue" screen.

                $parameter = @{
                    URI          = "{0}/assignee" -f $issueObj.RestUrl
                    Method       = "PUT"
                    Body         = ConvertTo-Json -InputObject $assigneeProps
                    Credential   = $Credential
                    GetParameter = $SkipNotificationParams
                }
                Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
                if ($PSCmdlet.ShouldProcess($issueObj.Key, "Updating Issue [Assignee] from JIRA")) {
                    Invoke-JiraMethod @parameter
                }
            }

            if ($Label) {
                Set-JiraIssueLabel -Issue $issueObj -Set $Label -Credential $Credential
            }

            if ($PassThru) {
                Get-JiraIssue -Key $issueObj.Key -Credential $Credential
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraIssueLabel {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, DefaultParameterSetName = 'ReplaceLabels' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('Key')]
        [Object[]]
        $Issue,

        [Parameter( Mandatory, ParameterSetName = 'ReplaceLabels' )]
        [Alias('Label', 'Replace')]
        [String[]]
        $Set,

        [Parameter( ParameterSetName = 'ModifyLabels' )]
        [String[]]
        $Add,

        [Parameter( ParameterSetName = 'ModifyLabels' )]
        [String[]]
        $Remove,

        [Parameter( Mandatory, ParameterSetName = 'ClearLabels' )]
        [Switch]
        $Clear,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_issue in $Issue) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_issue]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_issue [$_issue]"

            # Find the proper object for the Issue
            $issueObj = Resolve-JiraIssueObject -InputObject $_issue -Credential $Credential

            $labels = [System.Collections.ArrayList]@($issueObj.labels | Where-Object {$_})

            # As of JIRA 6.4, the Add and Remove verbs in the REST API for
            # updating issues do not support arrays of parameters - you
            # need to pass a single label to add or remove per API call.

            # Instead, we'll do some fancy footwork with the existing
            # issue object and use the Set verb for everything, so we only
            # have to make one call to JIRA.
            switch ($PSCmdlet.ParameterSetName) {
                'ClearLabels' {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Clearing all labels"
                    $labels = [System.Collections.ArrayList]@()
                }
                'ReplaceLabels' {
                    Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Replacing existing labels"
                    $labels = [System.Collections.ArrayList]$Set
                }
                'ModifyLabels' {
                    if ($Add) {
                        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Adding labels"
                        $null = foreach ($_add in $Add) { $labels.Add($_add) }
                    }
                    if ($Remove) {
                        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Removing labels"
                        foreach ($item in $Remove) {
                            $labels.Remove($item)
                        }
                    }
                }
            }

            $requestBody = @{
                'update' = @{
                    'labels' = @(
                        @{
                            'set' = @($labels)
                        }
                    )
                }
            }

            $parameter = @{
                URI        = $issueObj.RestURL
                Method     = "PUT"
                Body       = ConvertTo-Json -InputObject $requestBody -Depth 6
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($IssueObj.Key, "Updating Issue labels")) {
                Invoke-JiraMethod @parameter

                if ($PassThru) {
                    Get-JiraIssue -Key $issueObj.Key -Credential $Credential
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraUser {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess, DefaultParameterSetName = 'ByNamedParameters' )]
    param(
        [Parameter( Position = 0, Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.User" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraUser'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for User. Expected [JiraPS.User] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Alias('UserName')]
        [Object[]]
        $User,

        [Parameter( ParameterSetName = 'ByNamedParameters' )]
        [ValidateNotNullOrEmpty()]
        [String]
        $DisplayName,

        [Parameter( ParameterSetName = 'ByNamedParameters' )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if ($_ -match '^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$') {
                    return $true
                }
                else {
                    $exception = ([System.ArgumentException]"Invalid Argument") #fix code highlighting]
                    $errorId = 'ParameterValue.NotEmail'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $Issue
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "The value provided does not look like an email address."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    return $false
                }
            }
        )]
        [String]
        $EmailAddress,

        [Parameter( ParameterSetName = 'ByNamedParameters' )]
        [Boolean]
        $Active,

        [Parameter( Position = 1, Mandatory, ParameterSetName = 'ByHashtable' )]
        [Hashtable]
        $Property,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty,

        [Switch]
        $PassThru
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        $server = Get-JiraConfigServer -ErrorAction Stop

        $resourceURi = "$server/rest/api/2/user?username={0}"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_user in $User) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_user]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_user [$_user]"

            $userObj = Resolve-JiraUser -InputObject $_user -Exact -Credential $Credential -ErrorAction Stop

            $requestBody = @{}

            switch ($PSCmdlet.ParameterSetName) {
                'ByNamedParameters' {
                    if (-not ($DisplayName -or $EmailAddress -or $PSBoundParameters.ContainsKey('Active'))) {
                        $errorMessage = @{
                            Category         = "InvalidArgument"
                            CategoryActivity = "Validating Arguments"
                            Message          = "The parameters provided do not change the User. No action will be performed"
                        }
                        Write-Error @errorMessage
                        return
                    }

                    if ($DisplayName) {
                        $requestBody.displayName = $DisplayName
                    }

                    if ($EmailAddress) {
                        $requestBody.emailAddress = $EmailAddress
                    }

                    if ($PSBoundParameters.ContainsKey('Active')) {
                        $requestBody.active = $Active
                    }
                }
                'ByHashtable' {
                    $requestBody = $Property
                }
            }

            $parameter = @{
                URI        = $resourceURi -f $userObj.Name
                Method     = "PUT"
                Body       = ConvertTo-Json -InputObject $requestBody -Depth 4
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($UserObj.DisplayName, "Updating user")) {
                $result = Invoke-JiraMethod @parameter

                if ($PassThru) {
                    Write-Output (Get-JiraUser -InputObject $result)
                }
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Set-JiraVersion {
    # .ExternalHelp ..\JiraPS-help.xml
    [CmdletBinding( SupportsShouldProcess )]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Version" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraVersion'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Version. Expected [JiraPS.Version] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object[]]
        $Version,

        [String]
        $Name,

        [String]
        $Description,

        [Bool]
        $Archived,

        [Bool]
        $Released,

        [DateTime]
        $ReleaseDate,

        [DateTime]
        $StartDate,

        [ValidateScript(
            {
                if (("JiraPS.Project" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraProject'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Project. Expected [JiraPS.Project] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                    <#
                      #ToDo:CustomClass
                      Once we have custom classes, this check can be done with Type declaration
                    #>
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $Project,

        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"
    }

    process {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        foreach ($_version in $Version) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Processing [$_version]"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Processing `$_version [$_version]"

            $versionObj = Get-JiraVersion -Id $_version.Id -Credential $Credential -ErrorAction Stop

            $requestBody = @{}

            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Name")) {
                $requestBody["name"] = $Name
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Description")) {
                $requestBody["description"] = $Description
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Archived")) {
                $requestBody["archived"] = $Archived
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Released")) {
                $requestBody["released"] = $Released
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("Project")) {
                $projectObj = Get-JiraProject -Project $Project -Credential $Credential -ErrorAction Stop

                $requestBody["projectId"] = $projectObj.Id
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("ReleaseDate")) {
                $requestBody["releaseDate"] = $ReleaseDate.ToString('yyyy-MM-dd')
            }
            if ($PSCmdlet.MyInvocation.BoundParameters.ContainsKey("StartDate")) {
                $requestBody["startDate"] = $StartDate.ToString('yyyy-MM-dd')
            }

            $parameter = @{
                URI        = $versionObj.RestUrl
                Method     = "PUT"
                Body       = ConvertTo-Json -InputObject $requestBody
                Credential = $Credential
            }
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Invoking JiraMethod with `$parameter"
            if ($PSCmdlet.ShouldProcess($Name, "Updating Version on JIRA")) {
                $result = Invoke-JiraMethod @parameter

                Write-Output (ConvertTo-JiraVersion -InputObject $result)
            }
        }
    }

    end {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Complete"
    }
}

function Convert-Result {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        $InputObject,

        $OutputType
    )

    process {
        $InputObject | ForEach-Object {
            $item = $_
            if ($OutputType) {
                $converter = "ConvertTo-$OutputType"
            }

            if ($converter -and (Test-Path function:\$converter)) {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Outputting `$result as $OutputType"
                $item | & $converter
            }
            else {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Outputting `$result"
                $item
            }
        }
    }
}

if ($PSVersionTable.PSVersion.Major -lt 6) {
    function ConvertFrom-Json {
        <#
    .SYNOPSIS
        Function to overwrite or be used instead of the native `ConvertFrom-Json` of PowerShell
    .DESCRIPTION
        ConvertFrom-Json implementation does not allow for overriding JSON maxlength.
        The default limit is easy to exceed with large issue lists.
    #>
        [CmdletBinding()]
        param(
            [Parameter( Mandatory, ValueFromPipeline )]
            [Object[]]
            $InputObject,

            [Int]
            $MaxJsonLength = [Int]::MaxValue
        )

        begin {
            function ConvertFrom-Dictionary {
                param(
                    [System.Collections.Generic.IDictionary`2[String, Object]]$InputObject
                )

                process {
                    $returnObject = New-Object PSObject

                    foreach ($key in $InputObject.Keys) {
                        $pairObjectValue = $InputObject[$key]

                        if ($pairObjectValue -is [System.Collections.Generic.IDictionary`2].MakeGenericType([String], [Object])) {
                            $pairObjectValue = ConvertFrom-Dictionary $pairObjectValue
                        }
                        elseif ($pairObjectValue -is [System.Collections.Generic.ICollection`1].MakeGenericType([Object])) {
                            $pairObjectValue = ConvertFrom-Collection $pairObjectValue
                        }

                        $returnObject | Add-Member Noteproperty $key $pairObjectValue
                    }

                    return $returnObject
                }
            }

            function ConvertFrom-Collection {
                param(
                    [System.Collections.Generic.ICollection`1[Object]]$InputObject
                )

                process {
                    $returnList = New-Object ([System.Collections.Generic.List`1].MakeGenericType([Object]))
                    foreach ($jsonObject in $InputObject) {
                        $jsonObjectValue = $jsonObject

                        if ($jsonObjectValue -is [System.Collections.Generic.IDictionary`2].MakeGenericType([String], [Object])) {
                            $jsonObjectValue = ConvertFrom-Dictionary $jsonObjectValue
                        }
                        elseif ($jsonObjectValue -is [System.Collections.Generic.ICollection`1].MakeGenericType([Object])) {
                            $jsonObjectValue = ConvertFrom-Collection $jsonObjectValue
                        }

                        $returnList.Add($jsonObjectValue) | Out-Null
                    }

                    return $returnList.ToArray()
                }
            }

            $scriptAssembly = [System.Reflection.Assembly]::LoadWithPartialName("System.Web.Extensions")

            $typeResolver = @"
public class JsonObjectTypeResolver : System.Web.Script.Serialization.JavaScriptTypeResolver
{
	public override System.Type ResolveType(string id)
	{
		return typeof (System.Collections.Generic.Dictionary<string, object>);
	}

	public override string ResolveTypeId(System.Type type)
	{
		return string.Empty;
	}
}
"@

            try {
                Add-Type -TypeDefinition $typeResolver -ReferencedAssemblies $scriptAssembly.FullName
            }
            catch {
                # This is a relatively common error that's harmless unless changing the actual C#
                # code, so it can be ignored. Unfortunately, it's just a plain old System.Exception,
                # so we can't try to catch a specific error type.
                if (-not $_.ToString() -like "*The type name 'JsonObjectTypeResolver' already exists*") {
                    throw $_
                }
            }

            $jsonserial = New-Object System.Web.Script.Serialization.JavaScriptSerializer(New-Object JsonObjectTypeResolver)
            $jsonserial.MaxJsonLength = $MaxJsonLength
        }

        process {
            foreach ($i in $InputObject) {
                $s = $i.ToString()
                if ($s) {
                    $jsonTree = $jsonserial.DeserializeObject($s)

                    if ($jsonTree -is [System.Collections.Generic.IDictionary`2].MakeGenericType([String], [Object])) {
                        $jsonTree = ConvertFrom-Dictionary $jsonTree
                    }
                    elseif ($jsonTree -is [System.Collections.Generic.ICollection`1].MakeGenericType([Object])) {
                        $jsonTree = ConvertFrom-Collection $jsonTree
                    }

                    Write-Output $jsonTree
                }
            }
        }
    }
}

function ConvertFrom-URLEncoded {
    <#
    .SYNOPSIS
        Decode a URL encoded string
    #>
    [CmdletBinding()]
    [OutputType([String])]
    param (
        # String to decode
        [Parameter( Mandatory, ValueFromPipeline )]
        [String[]]
        $InputString
    )

    process {
        foreach ($input in $InputString) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Decoding string from URL"
            [System.Web.HttpUtility]::UrlDecode($input)
        }
    }
}

function ConvertTo-GetParameter {
    <#
    .SYNOPSIS
    Generate the GET parameter string for an URL from a hashtable
    #>
    [CmdletBinding()]
    param (
        [Parameter( Position = 0, Mandatory = $true, ValueFromPipeline = $true )]
        [hashtable]$InputObject
    )

    process {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Making HTTP get parameter string out of a hashtable"
        Write-Verbose ($InputObject | Out-String)
        [string]$parameters = "?"
        foreach ($key in $InputObject.Keys) {
            $value = $InputObject[$key]
            $parameters += "$key=$($value)&"
        }
        $parameters -replace ".$"
    }
}

function ConvertTo-JiraAttachment {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'        = $i.id
                'Self'      = $i.self
                'FileName'  = $i.FileName
                'Author'    = ConvertTo-JiraUser -InputObject $i.Author
                'Created'   = Get-Date -Date ($i.created)
                'Size'      = ([Int]$i.size)
                'MimeType'  = $i.mimeType
                'Content'   = $i.content
                'Thumbnail' = $i.thumbnail
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Attachment')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.FileName)"
            }

            Write-Output $result
        }
    }
}



function ConvertTo-JiraComment {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'         = $i.id
                'Body'       = $i.body
                'Visibility' = $i.visibility
                'RestUrl'    = $i.self
            }

            if ($i.author) {
                $props.Author = ConvertTo-JiraUser -InputObject $i.author
            }

            if ($i.updateAuthor) {
                $props.UpdateAuthor = ConvertTo-JiraUser -InputObject $i.updateAuthor
            }

            if ($i.created) {
                $props.Created = (Get-Date ($i.created))
            }

            if ($i.updated) {
                $props.Updated = (Get-Date ($i.updated))
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Comment')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Body)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraComponent {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'RestUrl'     = $i.self
                'Lead'        = $i.lead
                'ProjectName' = $i.project
                'ProjectId'   = $i.projectId
            }

            if ($i.lead) {
                $props.Lead = $i.lead
                $props.LeadDisplayName = $i.lead.displayName
            }
            else {
                $props.Lead = $null
                $props.LeadDisplayName = $null
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Component')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraCreateMetaField {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $fields = $i.projects.issuetypes.fields
            $fieldNames = (Get-Member -InputObject $fields -MemberType '*Property').Name
            foreach ($f in $fieldNames) {
                $item = $fields.$f

                $props = @{
                    'Id'              = $f
                    'Name'            = $item.name
                    'HasDefaultValue' = [System.Convert]::ToBoolean($item.hasDefaultValue)
                    'Required'        = [System.Convert]::ToBoolean($item.required)
                    'Schema'          = $item.schema
                    'Operations'      = $item.operations
                }

                if ($item.allowedValues) {
                    $props.AllowedValues = $item.allowedValues
                }

                if ($item.autoCompleteUrl) {
                    $props.AutoCompleteUrl = $item.autoCompleteUrl
                }

                foreach ($extraProperty in (Get-Member -InputObject $item -MemberType NoteProperty).Name) {
                    if ($null -eq $props.$extraProperty) {
                        $props.$extraProperty = $item.$extraProperty
                    }
                }

                $result = New-Object -TypeName PSObject -Property $props
                $result.PSObject.TypeNames.Insert(0, 'JiraPS.CreateMetaField')
                $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                    Write-Output "$($this.Name)"
                }

                Write-Output $result
            }
        }
    }
}

function ConvertTo-JiraEditMetaField {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $fields = $i.fields
            $fieldNames = (Get-Member -InputObject $fields -MemberType '*Property').Name
            foreach ($f in $fieldNames) {
                $item = $fields.$f

                $props = @{
                    'Id'              = $f
                    'Name'            = $item.name
                    'HasDefaultValue' = [System.Convert]::ToBoolean($item.hasDefaultValue)
                    'Required'        = [System.Convert]::ToBoolean($item.required)
                    'Schema'          = $item.schema
                    'Operations'      = $item.operations
                }

                if ($item.allowedValues) {
                    $props.AllowedValues = $item.allowedValues
                }

                if ($item.autoCompleteUrl) {
                    $props.AutoCompleteUrl = $item.autoCompleteUrl
                }

                foreach ($extraProperty in (Get-Member -InputObject $item -MemberType NoteProperty).Name) {
                    if ($null -eq $props.$extraProperty) {
                        $props.$extraProperty = $item.$extraProperty
                    }
                }

                $result = New-Object -TypeName PSObject -Property $props
                $result.PSObject.TypeNames.Insert(0, 'JiraPS.EditMetaField')
                $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                    Write-Output "$($this.Name)"
                }

                Write-Output $result
            }
        }
    }
}

function ConvertTo-JiraField {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'Custom'      = [System.Convert]::ToBoolean($i.custom)
                'Orderable'   = [System.Convert]::ToBoolean($i.orderable)
                'Navigable'   = [System.Convert]::ToBoolean($i.navigable)
                'Searchable'  = [System.Convert]::ToBoolean($i.searchable)
                'ClauseNames' = $i.clauseNames
                'Schema'      = $i.schema
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Field')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraFilter {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject,

        [PSObject[]]
        $FilterPermissions
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'                = $i.id
                'Name'              = $i.name
                'JQL'               = $i.jql
                'RestUrl'           = $i.self
                'ViewUrl'           = $i.viewUrl
                'SearchUrl'         = $i.searchUrl
                'Favourite'         = $i.favourite
                'FilterPermissions' = @()

                'SharePermission'   = $i.sharePermissions
                'SharedUser'        = $i.sharedUsers
                'Subscription'      = $i.subscriptions
            }

            if ($FilterPermissions) {
                $props.FilterPermissions = @(ConvertTo-JiraFilterPermission ($FilterPermissions))
            }

            if ($i.description) {
                $props.Description = $i.description
            }

            if ($i.owner) {
                $props.Owner = ConvertTo-JiraUser -InputObject $i.owner
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Filter')
            $result | Add-Member -MemberType ScriptMethod -Name 'ToString' -Force -Value {
                Write-Output "$($this.Name)"
            }
            $result | Add-Member -MemberType AliasProperty -Name 'Favorite' -Value 'Favourite'

            Write-Output $result
        }
    }
}

function ConvertTo-JiraFilterPermission {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'      = $i.id
                'Type'    = $i.type
                'Group'   = $null
                'Project' = $null
                'Role'    = $null
            }
            if ($i.group) {
                $props["Group"] = ConvertTo-JiraGroup $i.group
            }
            if ($i.project) {
                $props["Project"] = ConvertTo-JiraProject $i.project
            }
            if ($i.role) {
                $props["Role"] = ConvertTo-JiraProjectRole $i.role
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.FilterPermission')
            $result | Add-Member -MemberType ScriptMethod -Name 'ToString' -Force -Value {
                Write-Output "$($this.Type)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraGroup {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'Name'    = $i.name
                'RestUrl' = $i.self
            }

            if ($i.users) {
                $props.Size = $i.users.size

                if ($i.users.items) {
                    $allUsers = New-Object -TypeName System.Collections.ArrayList
                    foreach ($user in $i.users.items) {
                        [void] $allUsers.Add( (ConvertTo-JiraUser -InputObject $user) )
                    }

                    $props.Member = ($allUsers.ToArray())
                }
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Group')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraIssue {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject,

        [Switch]
        $IncludeDebug
    )

    begin {
        $userFields = @('Assignee', 'Creator', 'Reporter')
        $dateFields = @('Created', 'LastViewed', 'Updated')

        $transitions = New-Object -TypeName System.Collections.ArrayList
        $comments = New-Object -TypeName System.Collections.ArrayList
    }

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            [void] $transitions.Clear()
            [void] $comments.Clear()

            $http = "{0}browse/$($i.key)" -f ($InputObject.self -split 'rest')[0]

            $props = @{
                'ID'          = $i.id
                'Key'         = $i.key
                'HttpUrl'     = $http
                'RestUrl'     = $i.self
                'Summary'     = $i.fields.summary
                'Description' = $i.fields.description
                'Status'      = $i.fields.status.name
            }

            if ($i.fields.issuelinks) {
                $props['IssueLinks'] = ConvertTo-JiraIssueLink -InputObject $i.fields.issuelinks
            }

            if ($i.fields.attachment) {
                $props["Attachment"] = ConvertTo-JiraAttachment $i.fields.attachment
            }

            if ($i.fields.project) {
                $props.Project = ConvertTo-JiraProject -InputObject $i.fields.project
            }

            foreach ($field in $userFields) {
                if ($i.fields.$field) {
                    $props.$field = ConvertTo-JiraUser -InputObject $i.fields.$field
                }
                elseif ($field -eq 'Assignee') {
                    $props.Assignee = 'Unassigned'
                }
                else {
                }
            }

            foreach ($field in $dateFields) {
                if ($i.fields.$field) {
                    $props.$field = Get-Date -Date ($i.fields.$field)
                }
            }

            if ($IncludeDebug) {
                $props.Fields = $i.fields
                $props.Expand = $i.expand
            }

            [void] $transitions.Clear()
            foreach ($t in $i.transitions) {
                [void] $transitions.Add( (ConvertTo-JiraTransition -InputObject $t) )
            }
            $props.Transition = $transitions.ToArray()

            [void] $comments.Clear()
            if ($i.fields.comment) {
                if ($i.fields.comment.comments) {
                    foreach ($c in $i.fields.comment.comments) {
                        [void] $comments.Add( (ConvertTo-JiraComment -InputObject $c) )
                    }
                    $props.Comment = $comments.ToArray()
                }
            }

            $extraFields = $i.fields.PSObject.Properties | Where-Object -FilterScript { $_.Name -notin $props.Keys }
            foreach ($f in $extraFields) {
                $name = $f.Name
                $props[$name] = $f.Value
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Issue')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "[$($this.Key)] $($this.Summary)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraIssueLink {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'   = $i.id
                'Type' = ConvertTo-JiraIssueLinkType $i.type
            }

            if ($i.inwardIssue) {
                $props['InwardIssue'] = ConvertTo-JiraIssue $i.inwardIssue
            }

            if ($i.outwardIssue) {
                $props['OutwardIssue'] = ConvertTo-JiraIssue $i.outwardIssue
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.IssueLink')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.ID)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraIssueLinkType {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'InwardText'  = $i.inward
                'OutwardText' = $i.outward
                'RestUrl'     = $i.self
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.IssueLinkType')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraIssueType {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'Description' = $i.description
                'IconUrl'     = $i.iconUrl
                'RestUrl'     = $i.self
                'Subtask'     = [System.Convert]::ToBoolean($i.subtask)
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.IssueType')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraLink {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'Id'      = $i.id
                'RestUrl' = $i.self
            }

            if ($i.globalId) {
                $props.globalId = $i.globalId
            }

            if ($i.application) {
                $props.application = New-Object PSObject -Prop @{
                    type = $i.application.type
                    name = $i.application.name
                }
            }

            if ($i.relationship) {
                $props.relationship = $i.relationship
            }

            if ($i.object) {
                if ($i.object.icon) {
                    $icon = New-Object PSObject -Prop @{
                        title    = $i.object.icon.title
                        url16x16 = $i.object.icon.url16x16
                    }
                }
                else { $icon = $null }

                if ($i.object.status.icon) {
                    $statusIcon = New-Object PSObject -Prop @{
                        link     = $i.object.status.icon.link
                        title    = $i.object.status.icon.title
                        url16x16 = $i.object.status.icon.url16x16
                    }
                }
                else { $statusIcon = $null }

                if ($i.object.status) {
                    $status = New-Object PSObject -Prop @{
                        resolved = $i.object.status.resolved
                        icon     = $statusIcon
                    }
                }
                else { $status = $null }

                $props.object = New-Object PSObject -Prop @{
                    url     = $i.object.url
                    title   = $i.object.title
                    summary = $i.object.summary
                    icon    = $icon
                    status  = $status
                }
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Link')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Id)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraPriority {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'Description' = $i.description
                'StatusColor' = $i.statusColor
                'IconUrl'     = $i.iconUrl
                'RestUrl'     = $i.self
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Priority')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraProject {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Key'         = $i.key
                'Name'        = $i.name
                'Description' = $i.description
                'Lead'        = ConvertTo-JiraUser $i.lead
                'IssueTypes'  = ConvertTo-JiraIssueType $i.issueTypes
                'Roles'       = $i.roles
                'RestUrl'     = $i.self
                'Components'  = $i.components
                'Style'       = $i.style
            }

            if ($i.projectCategory) {
                $props.Category = $i.projectCategory
            }
            elseif ($i.Category) {
                $props.Category = $i.Category
            }
            else {
                $props.Category = $null
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Project')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraProjectRole {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'Description' = $i.description
                'Actors'      = $i.actors
                'RestUrl'     = $i.self
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.ProjectRole')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraServerInfo {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'BaseURL'        = $i.baseUrl
                # With PoSh v6, the version shall be casted to [SemanticVersion]
                'Version'        = $i.version
                'DeploymentType' = $i.deploymentType
                'BuildNumber'    = $i.buildNumber
                'BuildDate'      = Get-Date $i.buildDate
                'ServerTime'     = Get-Date $i.serverTime
                'ScmInfo'        = $i.scmInfo
                'ServerTitle'    = $i.serverTitle
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.ServerInfo')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "[$($this.DeploymentType)] $($this.Version)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraSession {
    [CmdletBinding()]
    param(
        [Parameter( Mandatory )]
        [Microsoft.PowerShell.Commands.WebRequestSession]
        $Session,

        [String]
        $Username
    )

    process {
        Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

        $props = @{
            'WebSession' = $Session
        }

        if ($Username) {
            $props.Username = $Username
        }

        $result = New-Object -TypeName PSObject -Property $props
        $result.PSObject.TypeNames.Insert(0, 'JiraPS.Session')
        $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
            Write-Output "JiraSession[JSessionID=$($this.JSessionID)]"
        }

        Write-Output $result
    }
}

function ConvertTo-JiraStatus {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Name'        = $i.name
                'Description' = $i.description
                'IconUrl'     = $i.iconUrl
                'RestUrl'     = $i.self
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Status')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraTransition {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'           = $i.id
                'Name'         = $i.name
                'ResultStatus' = ConvertTo-JiraStatus -InputObject $i.to
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Transition')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraUser {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'Key'          = $i.key
                'AccountId'    = $i.accountId
                'Name'         = $i.name
                'DisplayName'  = $i.displayName
                'EmailAddress' = $i.emailAddress
                'Active'       = [System.Convert]::ToBoolean($i.active)
                'AvatarUrl'    = $i.avatarUrls
                'TimeZone'     = $i.timeZone
                'Locale'       = $i.locale
                'Groups'       = $i.groups.items
                'RestUrl'      = $i.self
            }

            if ($i.groups) {
                $props.Groups = $i.groups.items.name
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.User')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraVersion {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'          = $i.id
                'Project'     = $i.projectId
                'Name'        = $i.name
                'Description' = $i.description
                'Archived'    = $i.archived
                'Released'    = $i.released
                'Overdue'     = $i.overdue
                'RestUrl'     = $i.self
            }

            if ($i.startDate) {
                $props["StartDate"] = Get-Date $i.startDate
            }
            else {
                $props["StartDate"] = ""
            }

            if ($i.releaseDate) {
                $props["ReleaseDate"] = Get-Date $i.releaseDate
            }
            else {
                $props["ReleaseDate"] = ""
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Version')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Name)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-JiraWorklogItem {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [PSObject[]]
        $InputObject
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            $props = @{
                'ID'         = $i.id
                'Visibility' = $i.visibility
                'Comment'    = $i.comment
                'RestUrl'    = $i.self
            }

            if ($i.author) {
                $props.Author = ConvertTo-JiraUser -InputObject $i.author
            }

            if ($i.updateAuthor) {
                $props.UpdateAuthor = ConvertTo-JiraUser -InputObject $i.updateAuthor
            }

            if ($i.created) {
                $props.Created = Get-Date ($i.created)
            }

            if ($i.updated) {
                $props.Updated = Get-Date ($i.updated)
            }

            if ($i.started) {
                $props.Started = Get-Date ($i.started)
            }

            if ($i.timeSpent) {
                $props.TimeSpent = $i.timeSpent
            }

            if ($i.timeSpentSeconds) {
                $props.TimeSpentSeconds = $i.timeSpentSeconds
            }

            $result = New-Object -TypeName PSObject -Property $props
            $result.PSObject.TypeNames.Insert(0, 'JiraPS.Worklogitem')
            $result | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                Write-Output "$($this.Id)"
            }

            Write-Output $result
        }
    }
}

function ConvertTo-ParameterHash {
    [CmdletBinding( DefaultParameterSetName = 'ByString' )]
    param (
        # URI from which to use the query
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName, ParameterSetName = 'ByUri' )]
        [Uri]
        $Uri,

        # Query string
        [Parameter( Position = 0, Mandatory, ParameterSetName = 'ByString' )]
        [String]
        $Query
    )

    process {
        $GetParameter = @{}

        if ($Uri) {
            $Query = $Uri.Query
        }

        if ($Query -match "^\?.+") {
            $Query.TrimStart("?").Split("&") | ForEach-Object {
                $key, $value = $_.Split("=")
                $GetParameter.Add($key, $value)
            }
        }

        Write-Output $GetParameter
    }
}

function ConvertTo-URLEncoded {
    <#
    .SYNOPSIS
        Encode a string into URL (eg: %20 instead of " ")
    #>
    [CmdletBinding()]
    [OutputType([String])]
    param (
        # String to encode
        [Parameter( Mandatory, ValueFromPipeline )]
        [String[]]
        $InputString
    )

    process {
        foreach ($input in $InputString) {
            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Encoding string to URL"
            [System.Web.HttpUtility]::UrlEncode($input)
        }
    }
}

function Expand-Result {
    [CmdletBinding()]
    param(
        [Parameter( Mandatory, ValueFromPipeline )]
        $InputObject
    )

    process {
        foreach ($container in $script:PagingContainers) {
            if (($InputObject) -and ($InputObject | Get-Member -Name $container)) {
                Write-DebugMessage "Extracting data from [$container] containter"
                $InputObject.$container
            }
        }
    }
}

function Invoke-WebRequest {
    # For Version up to 5.1
    <#
    .ForwardHelpTargetName
        Microsoft.PowerShell.Utility\Invoke-WebRequest
    .ForwardHelpCategory
        Cmdlet
    #>
    [CmdletBinding(HelpUri = 'https://go.microsoft.com/fwlink/?LinkID=217035')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute(
        "PSAvoidUsingConvertToSecureStringWithPlainText",
        "",
        Justification = "Converting received plaintext token to SecureString"
    )]
    param(
        [switch]
        ${UseBasicParsing},

        [Parameter(Mandatory = $true, Position = 0)]
        [ValidateNotNullOrEmpty()]
        [uri]
        ${Uri},

        [Microsoft.PowerShell.Commands.WebRequestSession]
        ${WebSession},

        [Alias('SV')]
        [string]
        ${SessionVariable},

        [pscredential]
        [System.Management.Automation.CredentialAttribute()]
        ${Credential} = [System.Management.Automation.PSCredential]::Empty,

        [switch]
        ${UseDefaultCredentials},

        [ValidateNotNullOrEmpty()]
        [string]
        ${CertificateThumbprint},

        [ValidateNotNull()]
        [System.Security.Cryptography.X509Certificates.X509Certificate]
        ${Certificate},

        [string]
        ${UserAgent},

        [switch]
        ${DisableKeepAlive},

        [ValidateRange(0, 2147483647)]
        [int32]
        ${TimeoutSec},

        [System.Collections.IDictionary]
        ${Headers},

        [ValidateRange(0, 2147483647)]
        [int]
        ${MaximumRedirection},

        [Microsoft.PowerShell.Commands.WebRequestMethod]
        ${Method},

        [uri]
        ${Proxy},

        [pscredential]
        [System.Management.Automation.CredentialAttribute()]
        ${ProxyCredential} = [System.Management.Automation.PSCredential]::Empty,

        [switch]
        ${ProxyUseDefaultCredentials},

        [Parameter(ValueFromPipeline = $true)]
        [System.Object]
        ${Body},

        [string]
        ${ContentType},

        [ValidateSet('chunked', 'compress', 'deflate', 'gzip', 'identity')]
        [string]
        ${TransferEncoding},

        [string]
        ${InFile},

        [string]
        ${OutFile},

        [switch]
        ${PassThru})

    begin {
        if ($Credential -and ($Credential -ne [System.Management.Automation.PSCredential]::Empty)) {
            $SecureCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes(
                    $('{0}:{1}' -f $Credential.UserName, $Credential.GetNetworkCredential().Password)
                ))
            $Headers["Authorization"] = "Basic $($SecureCreds)"
            $PSBoundParameters.Remove("Credential")
        }

        if ($InFile) {
            $boundary = [System.Guid]::NewGuid().ToString()
            $enc = [System.Text.Encoding]::GetEncoding("iso-8859-1")
            $fileName = Split-Path -Path $InFile -Leaf
            $readFile = Get-Content -Path $InFile -Encoding Byte
            $fileEnc = $enc.GetString($readFile)
            $PSBoundParameters["Body"] = @'
--{0}
Content-Disposition: form-data; name="file"; filename="{1}"
Content-Type: application/octet-stream

{2}
--{0}--

'@ -f $boundary, $fileName, $fileEnc

            $PSBoundParameters["Headers"]['X-Atlassian-Token'] = 'nocheck'
            $PSBoundParameters["ContentType"] = "multipart/form-data; boundary=`"$boundary`""
            $null = $PSBoundParameters.Remove("InFile")
        }

        try {
            $outBuffer = $null
            if ($PSBoundParameters.TryGetValue('OutBuffer', [ref]$outBuffer)) {
                $PSBoundParameters['OutBuffer'] = 1
            }
            $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Microsoft.PowerShell.Utility\Invoke-WebRequest', [System.Management.Automation.CommandTypes]::Cmdlet)
            $scriptCmd = {& $wrappedCmd @PSBoundParameters }
            $steppablePipeline = $scriptCmd.GetSteppablePipeline($myInvocation.CommandOrigin)
            $steppablePipeline.Begin($PSCmdlet)
        }
        catch {
            throw
        }
    }

    process {
        try {
            $steppablePipeline.Process($_)
            if ($SessionVariable) {
                Set-Variable -Name $SessionVariable -Value (Get-Variable $SessionVariable).Value -Scope 1
            }
        }
        catch {
            throw
        }
    }

    end {
        try {
            $steppablePipeline.End()
        }
        catch {
            throw
        }
    }
}

if ($PSVersionTable.PSVersion.Major -ge 6) {
    function Invoke-WebRequest {
        #require -Version 6
        <#
        .ForwardHelpTargetName
            Microsoft.PowerShell.Utility\Invoke-WebRequest
        .ForwardHelpCategory
            Cmdlet
        #>
        [CmdletBinding(DefaultParameterSetName = 'StandardMethod', HelpUri = 'https://go.microsoft.com/fwlink/?LinkID=217035')]
        param(
            [switch]
            ${UseBasicParsing},

            [Parameter(Mandatory = $true, Position = 0)]
            [ValidateNotNullOrEmpty()]
            [uri]
            ${Uri},

            [Microsoft.PowerShell.Commands.WebRequestSession]
            ${WebSession},

            [Alias('SV')]
            [string]
            ${SessionVariable},

            [switch]
            ${AllowUnencryptedAuthentication},

            [Microsoft.PowerShell.Commands.WebAuthenticationType]
            ${Authentication},

            [pscredential]
            [System.Management.Automation.CredentialAttribute()]
            ${Credential},

            [switch]
            ${UseDefaultCredentials},

            [ValidateNotNullOrEmpty()]
            [string]
            ${CertificateThumbprint},

            [ValidateNotNull()]
            [X509Certificate]
            ${Certificate},

            [switch]
            ${SkipCertificateCheck},

            [Microsoft.PowerShell.Commands.WebSslProtocol]
            ${SslProtocol},

            [securestring]
            ${Token},

            [string]
            ${UserAgent},

            [switch]
            ${DisableKeepAlive},

            [ValidateRange(0, 2147483647)]
            [int32]
            ${TimeoutSec},

            [System.Collections.IDictionary]
            ${Headers},

            [ValidateRange(0, 2147483647)]
            [int]
            ${MaximumRedirection},

            [Parameter(ParameterSetName = 'StandardMethod')]
            [Parameter(ParameterSetName = 'StandardMethodNoProxy')]
            [Microsoft.PowerShell.Commands.WebRequestMethod]
            ${Method},

            [Parameter(ParameterSetName = 'CustomMethod', Mandatory = $true)]
            [Parameter(ParameterSetName = 'CustomMethodNoProxy', Mandatory = $true)]
            [Alias('CM')]
            [ValidateNotNullOrEmpty()]
            [string]
            ${CustomMethod},

            [Parameter(ParameterSetName = 'CustomMethodNoProxy', Mandatory = $true)]
            [Parameter(ParameterSetName = 'StandardMethodNoProxy', Mandatory = $true)]
            [switch]
            ${NoProxy},

            [Parameter(ParameterSetName = 'StandardMethod')]
            [Parameter(ParameterSetName = 'CustomMethod')]
            [uri]
            ${Proxy},

            [Parameter(ParameterSetName = 'StandardMethod')]
            [Parameter(ParameterSetName = 'CustomMethod')]
            [pscredential]
            [System.Management.Automation.CredentialAttribute()]
            ${ProxyCredential},

            [Parameter(ParameterSetName = 'StandardMethod')]
            [Parameter(ParameterSetName = 'CustomMethod')]
            [switch]
            ${ProxyUseDefaultCredentials},

            [Parameter(ValueFromPipeline = $true)]
            [System.Object]
            ${Body},

            [string]
            ${ContentType},

            [ValidateSet('chunked', 'compress', 'deflate', 'gzip', 'identity')]
            [string]
            ${TransferEncoding},

            [string]
            ${InFile},

            [string]
            ${OutFile},

            [switch]
            ${PassThru},

            [switch]
            ${PreserveAuthorizationOnRedirect},

            [switch]
            ${SkipHeaderValidation})

        begin {
            if ($Credential -and (-not ($Authentication))) {
                $PSBoundParameters["Authentication"] = "Basic"
            }
            if ($InFile) {
                $multipartContent = [System.Net.Http.MultipartFormDataContent]::new()
                $FileStream = [System.IO.FileStream]::new($InFile, [System.IO.FileMode]::Open)
                $fileHeader = [System.Net.Http.Headers.ContentDispositionHeaderValue]::new("form-data")
                $fileHeader.Name = "file"
                $fileHeader.FileName = ([System.io.FileInfo]$InFile).name
                $fileContent = [System.Net.Http.StreamContent]::new($FileStream)
                $fileContent.Headers.ContentDisposition = $fileHeader
                $fileContent.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse("application/octet-stream")
                $multipartContent.Add($fileContent)
                $PSBoundParameters["Headers"]['X-Atlassian-Token'] = 'nocheck'
                $PSBoundParameters["Body"] = $multipartContent
                $null = $PSBoundParameters.Remove("InFile")
            }
            try {
                $outBuffer = $null
                if ($PSBoundParameters.TryGetValue('OutBuffer', [ref]$outBuffer)) {
                    $PSBoundParameters['OutBuffer'] = 1
                }
                $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Microsoft.PowerShell.Utility\Invoke-WebRequest', [System.Management.Automation.CommandTypes]::Cmdlet)
                $scriptCmd = {& $wrappedCmd @PSBoundParameters }
                $steppablePipeline = $scriptCmd.GetSteppablePipeline($myInvocation.CommandOrigin)
                $steppablePipeline.Begin($PSCmdlet)
            }
            catch {
                throw
            }
        }

        process {
            try {
                $steppablePipeline.Process($_)
                if ($SessionVariable) {
                    Set-Variable -Name $SessionVariable -Value (Get-Variable $SessionVariable).Value -Scope 1
                }
            }
            catch {
                throw
            }
        }

        end {
            try {
                $steppablePipeline.End()
            }
            catch {
                throw
            }
        }
    }
}

function Join-Hashtable {
    <#
	.SYNOPSIS
		Combines multiple hashtables into a single table.

	.DESCRIPTION
		Combines multiple hashtables into a single table.
		On multiple identic keys, the last wins.

	.EXAMPLE
		PS C:\> Join-Hashtable -Hashtable $Hash1, $Hash2

		Merges the hashtables contained in $Hash1 and $Hash2 into a single hashtable.
#>
    [CmdletBinding()]
    Param (
        # The tables to merge.
        [Parameter( Mandatory, ValueFromPipeline )]
        [AllowNull()]
        [System.Collections.IDictionary[]]
        $Hashtable
    )
    begin {
        $table = @{ }
    }

    process {
        foreach ($item in $Hashtable) {
            foreach ($key in $item.Keys) {
                $table[$key] = $item[$key]
            }
        }
    }

    end {
        $table
    }
}

function Resolve-DefaultParameterValue {
    <#
	.SYNOPSIS
		Used to filter and process default parameter values.

	.DESCRIPTION
		This command picks all the default parameter values from a reference hashtable.
		It then filters all that match a specified command and binds them to that specific command, narrowing its focus.
		These get merged into either a new or a specified hashtable and returned.

	.EXAMPLE
		PS C:\> Resolve-DefaultParameterValue -Reference $global:PSDefaultParameterValues -CommandName 'Invoke-WebRequest'

		Returns a hashtable containing all default parameter values in the global scope affecting the command 'Invoke-WebRequest'.
#>
    [CmdletBinding()]
    param (
        # The hashtable to pick default parameter valeus from.
        [Parameter( Mandatory )]
        [Hashtable]
        $Reference,

        # The commands to pick default parameter values for.
        [Parameter( Mandatory )]
        [String[]]
        $CommandName,

        # The target hashtable to merge results into.
        # By default an empty hashtable is used.
        [Hashtable]
        $Target = @{ },

        # Only resolve for specific parameter names.
        [String[]]
        $ParameterName = "*"
    )

    begin {
        $defaultItems = New-Object -TypeName System.Collections.ArrayList

        foreach ($key in $Reference.Keys) {
            $null = $defaultItems.Add(
                [PSCustomObject]@{
                    Key       = $key
                    Value     = $Reference[$key]
                    Command   = $key.Split(":")[0]
                    Parameter = $key.Split(":")[1]
                }
            )
        }
    }

    process {
        foreach ($command in $CommandName) {
            foreach ($item in $defaultItems) {
                if ($command -notlike $item.Command) { continue }

                foreach ($parameter in $ParameterName) {
                    if ($item.Parameter -like $parameter) {
                        if ($parameter -ne "*") {
                            $Target["$($command):$($parameter)"] = $item.Value
                        }
                        else {
                            $Target["$($command):$($item.Parameter)"] = $item.Value
                        }
                    }
                }
            }
        }
    }

    end {
        $Target
    }
}

function Resolve-ErrorWebResponse {
    [CmdletBinding()]
    param (
        $Exception,

        $StatusCode,

        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $PSCmdlet
    )

    begin {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function started"

        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] ParameterSetName: $($PsCmdlet.ParameterSetName)"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] PSBoundParameters: $($PSBoundParameters | Out-String)"

        # Powershell v6+ populates the body of the response into the exception
        if ($Exception.ErrorDetails) {
            $responseBody = $Exception.ErrorDetails.Message
        }
        # Powershell v5.1- has the body of the response in a Stream in the Exception Response
        else {
            $readStream = New-Object -TypeName System.IO.StreamReader -ArgumentList ($Exception.Exception.Response.GetResponseStream())
            $responseBody = $readStream.ReadToEnd()
            $readStream.Close()
        }

        $exception = "Invalid Server Response"
        $errorId = "InvalidResponse.Status$($StatusCode.value__)"
        $errorCategory = "InvalidResult"

        if ($responseBody) {
            # Clear the body in case it is not a JSON (but rather html)
            if ($responseBody -match "^[\s\t]*\<html\>") {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Content is HTML - replacing it with a generic json"
                $responseBody = '{"errorMessages": "Invalid server response. HTML returned."}'
            }

            Write-Verbose "[$($MyInvocation.MyCommand.Name)] Retrieved body of HTTP response for more information about the error (`$responseBody)"
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Got the following error as `$responseBody"

            try {
                $responseObject = ConvertFrom-Json -InputObject $responseBody -ErrorAction Stop

                foreach ($_error in ($responseObject.errorMessages + $responseObject.errors)) {
                    # $_error is a PSCustomObject - therefore can't be $false
                    if ($_error -is [PSCustomObject]) {
                        [String]$_error = ($_error | Out-String)
                    }
                    if (-not $_error) { throw "Unable to handle error" }

                    $writeErrorSplat = @{
                        Exception    = $exception
                        ErrorId      = $errorId
                        Category     = $errorCategory
                        Message      = $_error
                        TargetObject = $targetObject
                        Cmdlet       = $Cmdlet
                    }
                    WriteError @writeErrorSplat
                }
            }
            catch [ArgumentException] {
                Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] `$responseBody could not be converted from JSON"
                $writeErrorSplat = @{
                    Exception    = $exception
                    ErrorId      = $errorId
                    Category     = $errorCategory
                    Message      = $responseBody
                    TargetObject = $targetObject
                    Cmdlet       = $Cmdlet
                }
                WriteError @writeErrorSplat
            }
            catch {
                $writeErrorSplat = @{
                    Exception    = $exception
                    ErrorId      = $errorId
                    Category     = $errorCategory
                    Message      = "An unknown error ocurred."
                    TargetObject = $targetObject
                    Cmdlet       = $Cmdlet
                }
                WriteError @writeErrorSplat
            }
        }
        else {
            Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Response had no Body. Using `$StatusCode for generic error"
            $writeErrorSplat = @{
                Exception    = $exception
                ErrorId      = $errorId
                Category     = $errorCategory
                Message      = "Server responsed with $StatusCode"
                Cmdlet       = $Cmdlet
            }
            WriteError @writeErrorSplat
        }

        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Function ended"
    }
}

function Resolve-FilePath {
    <#
    .SYNOPSIS
        Resolve a path to it's full path

    .DESCRIPTION
        Resolve relative paths and PSDrive paths to the full path

    .LINK
        https://github.com/pester/Pester/blob/5796c95e4d6ff5528b8e14865e3f25e40f01bd65/Functions/TestResults.ps1#L13-L27
    #>
    [CmdletBinding()]
    param(
        # Path to be resolved
        [Parameter( Mandatory, ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [Alias("PSPath", "LiteralPath")]
        [String]
        $Path
    )

    process {
        $folder = Split-Path -Path $Path -Parent
        $file = Split-Path -Path $Path -Leaf

        if ( -not ([String]::IsNullOrEmpty($folder))) {
            $folderResolved = Resolve-Path -Path $folder
        }
        else {
            $folderResolved = Resolve-Path -Path $ExecutionContext.SessionState.Path.CurrentFileSystemLocation
        }

        Join-Path -Path $folderResolved.ProviderPath -ChildPath $file
    }
}

function Resolve-FullPath {
    [CmdletBinding()]
    param (
        # Path to be resolved.
        # Can be realtive or absolute.
        # Resolves PSDrives
        [Parameter( Mandatory, ValueFromPipeline, ValueFromPipelineByPropertyName )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (-not (Test-Path $_ -PathType Leaf)) {
                    $errorItem = [System.Management.Automation.ErrorRecord]::new(
                        ([System.ArgumentException]"File not found"),
                        'ParameterValue.FileNotFound',
                        [System.Management.Automation.ErrorCategory]::ObjectNotFound,
                        $_
                    )
                    $errorItem.ErrorDetails = "No file could be found with the provided path '$_'."
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [Alias( 'FullName', 'PSPath' )]
        [String]
        $Path
    )

    process {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Resolving path $Path"
        $PSCmdlet.GetUnresolvedProviderPathFromPSPath($Path)
    }
}

function Resolve-JiraError {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [Object[]]
        $InputObject,

        # Write error results to the error stream (Write-Error) instead of to the output stream
        [Switch]
        $WriteError,

        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $PSCmdlet
    )

    process {
        foreach ($i in $InputObject) {
            Write-Debug "[$($MyInvocation.MyCommand.Name)] Converting `$InputObject to custom object"

            if ($i.errorMessages) {
                foreach ($e in $i.errorMessages) {
                    if ($WriteError) {
                        $exception = ([System.ArgumentException]"Server responded with Error")
                        $errorId = "ServerResponse"
                        $errorCategory = 'NotSpecified'
                        $errorTarget = $i
                        $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                        $errorItem.ErrorDetails = "Jira encountered an error: [$e]"
                        $Cmdlet.WriteError($errorItem)
                    }
                    else {
                        $obj = [PSCustomObject] @{
                            'Message' = $e
                        }

                        $obj.PSObject.TypeNames.Insert(0, 'JiraPS.Error')
                        $obj | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                            Write-Output "Jira error [$($this.Message)]"
                        }

                        Write-Output $obj
                    }
                }
            }
            elseif ($i.errors) {
                $keys = (Get-Member -InputObject $i.errors | Where-Object -FilterScript {$_.MemberType -eq 'NoteProperty'}).Name
                foreach ($k in $keys) {
                    if ($WriteError) {
                        $exception = ([System.ArgumentException]"Server responded with Error")
                        $errorId = "ServerResponse.$k"
                        $errorCategory = 'NotSpecified'
                        $errorTarget = $i
                        $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                        $errorItem.ErrorDetails = "Jira encountered an error: [$k] - $($i.errors.$k)"
                        $Cmdlet.WriteError($errorItem)
                    }
                    else {
                        $obj = [PSCustomObject] @{
                            'Key'     = $k
                            'Message' = $i.errors.$k
                        }

                        $obj.PSObject.TypeNames.Insert(0, 'JiraPS.Error')
                        $obj | Add-Member -MemberType ScriptMethod -Name "ToString" -Force -Value {
                            Write-Output "Jira error [$($this.ID)]: $($this.Message)"
                        }

                        Write-Output $obj
                    }
                }
            }
        }
    }
}

function Resolve-JiraIssueObject {
    <#
      #ToDo:CustomClass
      Once we have custom classes, this will no longer be necessary
    #>
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.Issue" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraIssue'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for Issue. Expected [JiraPS.Issue] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $InputObject,

        # Authentication credentials
        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    # As we are not able to use proper type casting in the parameters, this is a workaround
    # to extract the data from a JiraPS.Issue object
    # This shall be removed once we have custom classes for the module
    if ("JiraPS.Issue" -in $InputObject.PSObject.TypeNames -and $InputObject.RestURL) {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Using `$InputObject as object"
        return $InputObject
    }
    elseif ("JiraPS.Issue" -in $InputObject.PSObject.TypeNames -and $InputObject.Key) {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Resolve Issue to object"
        return (Get-JiraIssue -Key $InputObject.Key -Credential $Credential -ErrorAction Stop)
    }
    else {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Resolve Issue to object"
        return (Get-JiraIssue -Key $InputObject.ToString() -Credential $Credential -ErrorAction Stop)
    }
}

function Resolve-JiraUser {
    <#
      #ToDo:CustomClass
      Once we have custom classes, this will no longer be necessary
    #>
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript(
            {
                if (("JiraPS.User" -notin $_.PSObject.TypeNames) -and (($_ -isnot [String]))) {
                    $exception = ([System.ArgumentException]"Invalid Type for Parameter") #fix code highlighting]
                    $errorId = 'ParameterType.NotJiraUser'
                    $errorCategory = 'InvalidArgument'
                    $errorTarget = $_
                    $errorItem = New-Object -TypeName System.Management.Automation.ErrorRecord $exception, $errorId, $errorCategory, $errorTarget
                    $errorItem.ErrorDetails = "Wrong object type provided for User. Expected [JiraPS.User] or [String], but was $($_.GetType().Name)"
                    $PSCmdlet.ThrowTerminatingError($errorItem)
                }
                else {
                    return $true
                }
            }
        )]
        [Object]
        $InputObject,

        [Switch]
        $Exact,

        # Authentication credentials
        [Parameter()]
        [System.Management.Automation.PSCredential]
        [System.Management.Automation.Credential()]
        $Credential = [System.Management.Automation.PSCredential]::Empty
    )

    # As we are not able to use proper type casting in the parameters, this is a workaround
    # to extract the data from a JiraPS.Issue object
    # This shall be removed once we have custom classes for the module
    if ("JiraPS.User" -in $InputObject.PSObject.TypeNames) {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Using `$InputObject as object"
        return $InputObject
    }
    else {
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Resolve User to object"
        return (Get-JiraUser -UserName $InputObject -Exact:$Exact -Credential $Credential -ErrorAction Stop)
    }
}

function Set-TlsLevel {
    [CmdletBinding( SupportsShouldProcess = $false )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseShouldProcessForStateChangingFunctions', '')]
    param (
        [Parameter(Mandatory, ParameterSetName = 'Set')]
        [Switch]$Tls12,

        [Parameter(Mandatory, ParameterSetName = 'Revert')]
        [Switch]$Revert
    )

    begin {
        switch ($PSCmdlet.ParameterSetName) {
            "Set" {
                $Script:OriginalTlsSettings = [Net.ServicePointManager]::SecurityProtocol

                [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
            }
            "Revert" {
                if ($Script:OriginalTlsSettings) {
                    [Net.ServicePointManager]::SecurityProtocol = $Script:OriginalTlsSettings
                }
            }
        }
    }
}

function Test-ServerResponse {
    [CmdletBinding()]
    <#
        .SYNOPSIS
            Evauluate the response of the API call
        .LINK
            https://docs.atlassian.com/software/jira/docs/api/7.6.1/com/atlassian/jira/bc/security/login/LoginReason.html
    #>
    param (
        # Response of Invoke-WebRequest
        [Parameter( ValueFromPipeline )]
        [PSObject]$InputObject,

        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $PSCmdlet
    )

    begin {
        $loginReasonKey = "X-Seraph-LoginReason"
    }

    process {
        Write-Verbose "[$($MyInvocation.MyCommand.Name)] Checking response headers for authentication errors"
        Write-DebugMessage "[$($MyInvocation.MyCommand.Name)] Investigating `$InputObject.Headers['$loginReasonKey']"

        if ($InputObject.Headers -and $InputObject.Headers[$loginReasonKey]) {
            $loginReason = $InputObject.Headers[$loginReasonKey] -split ","

            switch ($true) {
                {$loginReason -contains "AUTHENTICATED_FAILED"} {
                    $errorParameter = @{
                        ExceptionType = "System.Net.Http.HttpRequestException"
                        Message       = "The user could not be authenticated."
                        ErrorId       = "AuthenticationFailed"
                        Category      = "AuthenticationError"
                        Cmdlet        = $Cmdlet
                    }
                    ThrowError @errorParameter
                }
                {$loginReason -contains "AUTHENTICATION_DENIED"} {
                    $errorParameter = @{
                        ExceptionType = "System.Net.Http.HttpRequestException"
                        Message       = "For security reasons Jira requires you to log on to the website before continuing."
                        ErrorId       = "AuthenticaionDenied"
                        Category      = "AuthenticationError"
                        Cmdlet        = $Cmdlet
                    }
                    ThrowError @errorParameter
                }
                {$loginReason -contains "AUTHORISATION_FAILED"} {
                    $errorParameter = @{
                        ExceptionType = "System.Net.Http.HttpRequestException"
                        Message       = "The user could not be authorised."
                        ErrorId       = "AuthorisationFailed"
                        Category      = "AuthenticationError"
                        Cmdlet        = $Cmdlet
                    }
                    ThrowError @errorParameter
                }
                {$loginReason -contains "OK"} {} # The login was OK
                {$loginReason -contains "OUT"} {} # This indicates that person has in fact logged "out"
            }
        }
    }

    end {
    }
}

function ThrowError {
    <#
    .SYNOPSIS
        Utility to throw a terminating errorrecord
    .NOTES
        Thanks to Jaykul:
        https://github.com/PoshCode/Configuration/blob/master/Source/Metadata.psm1
    #>
    param
    (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $((Get-Variable -Scope 1 PSCmdlet).Value),

        [Parameter(Mandatory = $true, ParameterSetName = "ExistingException", Position = 1, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [Parameter(ParameterSetName = "NewException")]
        [ValidateNotNullOrEmpty()]
        [System.Exception]
        $Exception,

        [Parameter(ParameterSetName = "NewException", Position = 2)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $ExceptionType = "System.Management.Automation.RuntimeException",

        [Parameter(Mandatory = $true, ParameterSetName = "NewException", Position = 3)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $Message,

        [Parameter(Mandatory = $false)]
        [System.Object]
        $TargetObject,

        [Parameter(Mandatory = $true, ParameterSetName = "ExistingException", Position = 10)]
        [Parameter(Mandatory = $true, ParameterSetName = "NewException", Position = 10)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $ErrorId,

        [Parameter(Mandatory = $true, ParameterSetName = "ExistingException", Position = 11)]
        [Parameter(Mandatory = $true, ParameterSetName = "NewException", Position = 11)]
        [ValidateNotNull()]
        [System.Management.Automation.ErrorCategory]
        $Category,

        [Parameter(Mandatory = $true, ParameterSetName = "Rethrow", Position = 1)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord
    )
    process {
        if (!$ErrorRecord) {
            if ($PSCmdlet.ParameterSetName -eq "NewException") {
                if ($Exception) {
                    $Exception = New-Object $ExceptionType $Message, $Exception
                }
                else {
                    $Exception = New-Object $ExceptionType $Message
                }
            }
            $errorRecord = New-Object System.Management.Automation.ErrorRecord $Exception, $ErrorId, $Category, $TargetObject
        }
        $Cmdlet.ThrowTerminatingError($errorRecord)
    }
}

function Write-DebugMessage {
    [CmdletBinding()]
    param(
        [Parameter( ValueFromPipeline )]
        [String]
        $Message
    )

    begin {
        $oldDebugPreference = $DebugPreference
        if (-not ($DebugPreference -eq "SilentlyContinue")) {
            $DebugPreference = 'Continue'
        }
    }

    process {
        Write-Debug $Message
    }

    end {
        $DebugPreference = $oldDebugPreference
    }
}

function WriteError {
    <#
    .SYNOPSIS
        Utility to write an errorrecord to the errstd
    .NOTES
        Thanks to Jaykul:
        https://github.com/PoshCode/Configuration/blob/master/Source/Metadata.psm1
    #>
    param
    (
        [Parameter()]
        [ValidateNotNullOrEmpty()]
        [System.Management.Automation.PSCmdlet]
        $Cmdlet = $((Get-Variable -Scope 1 PSCmdlet).Value),

        [Parameter(Mandatory = $true, ParameterSetName = "ExistingException", Position = 1, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [Parameter(ParameterSetName = "NewException")]
        [ValidateNotNullOrEmpty()]
        [System.Exception]
        $Exception,

        [Parameter(ParameterSetName = "NewException", Position = 2)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $ExceptionType = "System.Management.Automation.RuntimeException",

        [Parameter(Mandatory = $true, ParameterSetName = "NewException", Position = 3)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $Message,

        [Parameter(Mandatory = $false)]
        [System.Object]
        $TargetObject,

        [Parameter(Mandatory = $true, Position = 10)]
        [ValidateNotNullOrEmpty()]
        [System.String]
        $ErrorId,

        [Parameter(Mandatory = $true, Position = 11)]
        [ValidateNotNull()]
        [System.Management.Automation.ErrorCategory]
        $Category,

        [Parameter(Mandatory = $true, ParameterSetName = "Rethrow", Position = 1)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord
    )
    process {
        if (!$ErrorRecord) {
            if ($PSCmdlet.ParameterSetName -eq "NewException") {
                if ($Exception) {
                    $Exception = New-Object $ExceptionType $Message, $Exception
                }
                else {
                    $Exception = New-Object $ExceptionType $Message
                }
            }
            $errorRecord = New-Object System.Management.Automation.ErrorRecord $Exception, $ErrorId, $Category, $TargetObject
        }
        $Cmdlet.WriteError($errorRecord)
    }
}


