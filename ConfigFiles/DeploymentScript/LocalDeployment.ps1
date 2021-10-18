param([Boolean]$Launch = $false)

if ($Launch)
{
    Start-Process powershell.exe -Verb RunAs -ArgumentList ('-ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

#Lauch PS in project's folder
$Workspace = Split-Path -Path $MyInvocation.MyCommand.Path
$Workspace = [System.IO.Path]::GetFullPath("$Workspace\..\..").TrimEnd("\\")
Set-Location $Workspace

$ErrorActionPreference = "Stop"

function Publish-Worker-Service
{
    param(
        [String]$ProjectName = "LinkLookupBackgroundService"
    )

    $Path = "$ProjectName\$ProjectName.csproj"
    $OutputFolder = "$Workspace\Publishes\$ProjectName"
    if (Test-Path -Path $OutputFolder)
    {
        Remove-Item $OutputFolder -Recurse
    }

    Publish-Dotnet-Project $Path $OutputFolder
}

function Create-Link-Lookup-Windows-Service
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ServiceName,
        [Parameter(Position=1,mandatory=$true)]
        [String]$GroupName,
        [String]$ProjectName = "LinkLookupBackgroundService",
        [Switch]$Force
    )

    if (($Force) -and (Get-Service $ServiceName -ErrorAction SilentlyContinue))
    {
        Delete-Windows-Service $ServiceName
    }

    $OutputFolder = "$Workspace\Publishes\$ProjectName"
    sc.exe create "$ServiceName" binpath="$OutputFolder\$ProjectName.exe $GroupName"
}

function Create-Windows-Service
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ServiceName,
        [Parameter(Position=1,mandatory=$true)]
        [String]$BinariesPath
    )

    sc.exe create "$ServiceName" binpath=$BinariesPath
}

function Start-Windows-Service
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ServiceName
    )

    sc.exe start "$ServiceName"
}

function Stop-Windows-Service
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ServiceName
    )

    sc.exe stop "$ServiceName"
}

function Delete-Windows-Service
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ServiceName
    )

    sc.exe delete "$ServiceName"
}

function Publish-Dotnet-Project
{
    param(
        [Parameter(Position=0,mandatory=$true)]
        [String]$ProjectPath,
        [Parameter(Position=1,mandatory=$true)]
        [String]$OutputFolder,
        [String]$Runtime = "win-x64"
    )

    dotnet publish -c Debug $ProjectPath --output $OutputFolder --runtime $Runtime --self-contained false
}

function Write-Help
{
    Write-Output "*** Local Deployment avaliable commands ***"
    Write-Output "pws              (Publish-Worker-Service)                                           "
    Write-Output "cllws            (Create-Link-Lookup-Windows-Service) -Force deletes exising service"
    Write-Output "create-ws        (Create-Windows-Service)                                           "
    Write-Output "start-ws         (Start-Windows-Service)                                            "
    Write-Output "stop-ws          (Stop-Windows-Service)                                             "
    Write-Output "delete-ws        (Delete-Windows-Service)                                           "
    Write-Output "publish          (Publish-Dotnet-Project)                                           "
}

Set-Alias pws Publish-Worker-Service
Set-Alias cllws Create-Link-Lookup-Windows-Service
Set-Alias create-ws Create-Windows-Service
Set-Alias start-ws Start-Windows-Service
Set-Alias stop-ws Stop-Windows-Service
Set-Alias delete-ws Delete-Windows-Service
Set-Alias publish Publish-Dotnet-Project
Write-Help