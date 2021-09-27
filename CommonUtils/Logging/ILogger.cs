using System;

namespace CommonUtils.Logging
{
    public interface ILogger
    {
        void LogInfo(string logMessage);

        void LogError(Exception e);
    }
}
