param([Boolean]$Launch = $false)

if ($Launch)
{
    Invoke-Expression ('cmd /c start powershell -ExecutionPolicy Bypass -NoExit -Command ". {0}"' -f ($MyInvocation.MyCommand.Definition))
    Exit
}

$ErrorActionPreference = "Stop"

# $Workspace = Split-Path $MyInvocation.Command.Path



function Publish-Worker
{
    # param(
    #     [String]$ServiceName = $(throw 'Service name parameter is mandatory')
    # )

    dotnet publish 'NotificationBackgroundService/NotificationBackgroundService.csproj'  --output "C:\custom\publish\directory"
}