param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    # Invoke-Expression ('cmd /c start powershell -ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

#Lauch PS in project's folder
$Workspace = Split-Path -Path $MyInvocation.MyCommand.Path
$Workspace = Split-Path -Path $Workspace
$Workspace = Split-Path -Path $Workspace
Set-Location ($Workspace)

$ErrorActionPreference = "Stop"

function Publish-LinkLookupService
{
    $ProjectName = "LinkLookupBackgroundService"

    $Path = "$ProjectName\$ProjectName.csproj"
    $OutputFolder = "$Workspace\Publishes\$ProjectName"

    Publish-Dotnet-Project $Path $OutputFolder
    Create-Windows-Service $ProjectName "$OutputFolder\$ProjectName.exe"
}

function Create-Windows-Service
{
    param(
        [String]$ServiceName = $(throw 'Service name parameter is mandatory'),
        [String]$BinariesPath = $(throw 'Binaries path parameter is mandatory')
    )

    sc.exe create "$ServiceName" binpath=$BinariesPath
}

function Start-Windows-Service
{
    param(
        [String]$ServiceName = $(throw 'Service name parameter is mandatory')
    )

    sc.exe start "$ServiceName"
}

function Stop-Windows-Service
{
    param(
        [String]$ServiceName = $(throw 'Service name parameter is mandatory')
    )

    sc.exe stop "$ServiceName"
}

function Delete-Windows-Service
{
    param(
        [String]$ServiceName = $(throw 'Service name parameter is mandatory')
    )

    sc.exe delete "$ServiceName"
}

function Publish-Dotnet-Project
{
    param(
        [String]$ProjectPath = $(throw 'Project path parameter is mandatory'),
        [String]$OutputFolder = $(throw 'Output folder parameter is mandatory')
    )

    dotnet publish $ProjectPath --output $OutputFolder
}