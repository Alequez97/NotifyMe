using CommonUtils.Interfaces;
using System;
using System.IO;

namespace CommonUtils.Logging
{
    public class TextFileLogger : ILogger
    {
        private readonly string _logFilePath;

        public TextFileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void LogError(Exception e)
        {
            var currentLogDirectory = GetLogDirectoryName();
            CreateDirectoryIfNotExists(currentLogDirectory);

            File.WriteAllText($"{currentLogDirectory}/error_{GetLogFilePostfix()}", $"[{DateTime.Now}] {e.Message}");
        }

        public void LogInfo(string logMessage)
        {
            var currentLogDirectory = GetLogDirectoryName();
            CreateDirectoryIfNotExists(currentLogDirectory);

            File.WriteAllText($"{currentLogDirectory}/info_{GetLogFilePostfix()}", $"[{DateTime.Now}] {logMessage}");
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string GetLogFilePostfix()
        {
            return $"{DateTime.Now.ToString("HH_mm")}.txt";
        }

        private string GetLogDirectoryName()
        {
            return $"{_logFilePath}/{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}";
        }
    }
}
