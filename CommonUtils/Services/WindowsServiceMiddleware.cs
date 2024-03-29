﻿using CommonUtils.Interfaces;
using CommonUtils.Models;

namespace CommonUtils.Services
{
    public class WindowsServiceMiddleware : IBackgroundServiceMiddleware
    {
        private readonly PowerShellExecutor powerShell;

        public WindowsServiceMiddleware()
        {
            powerShell = new PowerShellExecutor();
        }

        public CommandLineResponse CreateService(string serviceName, string pathToExeFile)
        {
            string command = $"sc.exe create \"{serviceName}\" binpath={pathToExeFile}";
            return powerShell.Invoke(command);
        }

        public CommandLineResponse StartService(string serviceName)
        {
            string command = $"Start-Service {serviceName}";
            return powerShell.Invoke(command);
        }

        public CommandLineResponse StopService(string serviceName)
        {
            string command = $"Stop-Service {serviceName}";
            return powerShell.Invoke(command);
        }

        public CommandLineResponse DeleteService(string serviceName)
        {
            var command = $"sc.exe delete \"{serviceName}\"";
            return powerShell.Invoke(command);
        }

        public bool CheckThatServiceExists(string serviceName)
        {
            var command = $"Get-Service {serviceName}";
            var response = powerShell.Invoke(command);

            return (response.ResponseStatus == CommandLineResponseStatus.Success) ? response.StandartOutput.Contains("Status") : false;
        }

        public ServiceStatus GetServiceStatus(string serviceName)
        {
            if (!CheckThatServiceExists(serviceName))
            {
                return ServiceStatus.NotExists;
            }

            var command = $"Get-Service {serviceName} | Format-List -Property Status";
            var response = powerShell.Invoke(command);

            return response.StandartOutput.Contains("Running") ? ServiceStatus.Running : ServiceStatus.Stopped;
        }
    }
}
