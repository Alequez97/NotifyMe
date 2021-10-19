using CommonUtils.Models;

namespace CommonUtils.Services
{
    public class WindowsServiceMiddleware
    {
        private readonly PowerShellExecutor powerShell;

        public WindowsServiceMiddleware()
        {
            powerShell = new PowerShellExecutor();
        }

        public PowerShellResponse CreateWindowsService(string serviceName, string pathToExeFile)
        {
            string command = $"sc.exe create \"{serviceName}\" binpath={pathToExeFile}";
            return powerShell.Invoke(command);
        }

        public PowerShellResponse StartWindowsService(string serviceName)
        {
            string command = $"Start-Service {serviceName}";
            return powerShell.Invoke(command);
        }

        public PowerShellResponse StopWindowsService(string serviceName)
        {
            string command = $"Stop-Service {serviceName}";
            return powerShell.Invoke(command);
        }

        public PowerShellResponse DeleteWindowsSerice(string serviceName)
        {
            var command = $"sc.exe delete \"{serviceName}\"";
            return powerShell.Invoke(command);
        }

        public bool CheckThatServiceExists(string serviceName)
        {
            var command = $"Get-Service {serviceName}";
            var response = powerShell.Invoke(command);

            return (response.ResponseStatus == PowerShellResponseStatus.Success) ? response.StandartOutput.Contains("Status") : throw response.Exception;
        }
    }
}
