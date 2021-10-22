using CommonUtils.Models;

namespace CommonUtils.Services.Interfaces
{
    public interface IBackgroundServiceMiddleware
    {
        public CommandLineResponse CreateService(string serviceName, string pathToExeFile);

        public CommandLineResponse StartService(string serviceName);

        public CommandLineResponse StopService(string serviceName);

        public CommandLineResponse DeleteSerice(string serviceName);

        public bool CheckThatServiceExists(string serviceName);
    }
}