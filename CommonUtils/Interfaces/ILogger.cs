using System;

namespace CommonUtils.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string logMessage);

        void LogError(Exception e);
    }
}
