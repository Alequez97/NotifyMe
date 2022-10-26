using CommonUtils.Models;

namespace CommonUtils.Interfaces
{
    public interface IBackgroundServiceMiddleware
    {
        public CommandLineResponse CreateService(string serviceName, string pathToExeFile);

        public CommandLineResponse StartService(string serviceName);

        public CommandLineResponse StopService(string serviceName);

        public CommandLineResponse DeleteService(string serviceName);

        public bool CheckThatServiceExists(string serviceName);

        public ServiceStatus GetServiceStatus(string serviceName);
    }
}