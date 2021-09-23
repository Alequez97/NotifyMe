param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

#Lauch PS in project's folder
$Workspace = Split-Path -Path $MyInvocation.MyCommand.Path
$Workspace = [System.IO.Path]::GetFullPath("$Workspace\..\..").TrimEnd("\\")
cd $Workspace

$ErrorActionPreference = "Stop"

function Publish-Worker-Service
{
    param(
        [String]$ServiceName = "LinkLookupBackgroundService",
        [String]$ProjectName = "LinkLookupBackgroundService"
    )

    $Path = "$ProjectName\$ProjectName.csproj"
    $OutputFolder = "$Workspace\Publishes\$ProjectName"

    Publish-Dotnet-Project $Path $OutputFolder
    Create-Windows-Service $ServiceName "$OutputFolder\$ProjectName.exe"
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

function Write-Help
{
    Write "*** Local Deployment avaliable commands ***"
    Write "publish          (Publish-Dotnet-Project)            Publishes dotnet project. Params (Path, OutputFolder)"
}

Set-Alias publish Publish-Dotnet-Project
Write-Help