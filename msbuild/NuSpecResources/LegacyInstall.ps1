param($installPath, $toolsPath, $package, $project)

##
## Inserts a new or updates an existing dependentAssembly element for a specified assembly
##
Function AddOrUpdateBindingRedirect([System.IO.FileInfo] $file, [System.Xml.XmlElement[]] $assemblyConfigs, [System.Xml.XmlDocument] $config)
{
    $name = [System.IO.Path]::GetFileNameWithoutExtension($file)
    $assemblyName = [System.Reflection.AssemblyName]::GetAssemblyName($file)

    $assemblyConfig =  $assemblyConfigs | ? { $_.assemblyIdentity.Name -Eq $name } 

    if ($assemblyConfig -Eq $null) 
    { 
        #there is no existing binding configuration for the assembly, we need to create a new config element for it
        Write-Host "Adding binding redirect for $name".

        $matches = $regex.Matches($assemblyName.FullName)
        if ($matches.Count -gt 0)
        {
	        $publicKeyToken = $matches[0].Groups["publicKeyToken"].Value
	        $culture = $matches[0].Groups["culture"].Value
        }
        else 
        {
            Write-Host "Unable to figure out culture and publicKeyToken for $name"
	        $publicKeyToken = "null"
	        $culture = "neutral"
        }
    
        $assemblyIdentity = $config.CreateElement("assemblyIdentity", $ns)
        $assemblyIdentity.SetAttribute("name", $name)
        if (![String]::IsNullOrEmpty($publicKeyToken))
        {
	        $assemblyIdentity.SetAttribute("publicKeyToken", $publicKeyToken)
        }
        if (![String]::IsNullOrEmpty($culture))
        {
	        $assemblyIdentity.SetAttribute("culture", $culture)
        }
        
        $bindingRedirect = $config.CreateElement("bindingRedirect", $ns)
        $bindingRedirect.SetAttribute("oldVersion", "")
        $bindingRedirect.SetAttribute("newVersion", "")
        
        $assemblyConfig = $config.CreateElement("dependentAssembly", $ns)
        $assemblyConfig.AppendChild($assemblyIdentity) | Out-Null
        $assemblyConfig.AppendChild($bindingRedirect) | Out-Null

        #locate the assemblyBinding element and append the newly created dependentAssembly element
        $assemblyBinding = $config.configuration.runtime.ChildNodes | where {$_.Name -eq "assemblyBinding"}
        $assemblyBinding.AppendChild($assemblyConfig) | Out-Null
    } 
    else 
    {
        Write-Host "Updating binding redirect for $name"
    }

    $assemblyConfig.bindingRedirect.oldVersion = "0.0.0.0-" + $assemblyName.Version
    $assemblyConfig.bindingRedirect.newVersion = $assemblyName.Version.ToString()
}

Function GetOrCreateXmlElement([System.Xml.XmlElement]$parent, $elementName, $ns, $document)
{
    $child = $parent.$($elementName)
    if ($child -eq $null) 
    {
        $child = $document.CreateElement($elementName, $ns)
        $parent.AppendChild($child) | Out-Null
    }
    $child
}


[regex]$regex = '[\w\.]+,\sVersion=[\d\.]+,\sCulture=(?<culture>[\w-]+),\sPublicKeyToken=(?<publicKeyToken>\w+)'
$ns = "urn:schemas-microsoft-com:asm.v1"
$libPath = join-path $installPath "lib\net461"
$projectFile = Get-Item $project.FullName

#locate the project configuration file
$webConfigPath = join-path $projectFile.Directory.FullName "web.config"
$appConfigPath = join-path $projectFile.Directory.FullName "app.config"
if (Test-Path $webConfigPath) 
{
    $configPath = $webConfigPath
}
elseif (Test-Path $appConfigPath)
{
    $configPath = $appConfigPath
}
else 
{
    Write-Host "Unable to find a configuration file, binding redirect not configured."
    return
}

#load the configuration file for the project
$config = New-Object xml
$config.psbase.PreserveWhitespace = $true
$config.Load($configPath)

# assume that we have the configuration element and make sure we have all the other parents of the AssemblyIdentity element.
$configElement = $config.configuration
$runtimeElement = GetOrCreateXmlElement $configElement "runtime" $null $config
$assemblyBindingElement = GetOrCreateXmlElement $runtimeElement "assemblyBinding" $ns $config

if ($assemblyBindingElement.length -gt 1)
{
    for ($i=1; $i -lt $assemblyBindingElement.length; $i++) 
    {
        $assemblyBindingElement[0].InnerXml +=  $assemblyBindingElement[$i].InnerXml
        $runtimeElement.RemoveChild($assemblyBindingElement[$i])
    }
    $config.Save($configPath)
}

else 
{
    $assemblyBindingElement = @($assemblyBindingElement)
}

$assemblyConfigs = $assemblyBindingElement[0].ChildNodes | where {$_.GetType().Name -eq "XmlElement"}

#add/update binding redirects for assemblies in the current package
get-childItem "$libPath\*.dll" | % { AddOrUpdateBindingRedirect $_  $assemblyConfigs $config }

$config.Save($configPath)