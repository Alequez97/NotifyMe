using CommonUtils.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CommonUtils.ConfigReader
{
    public class JsonImmutableConfigReader : IImmutableConfigReader
    {
        private readonly string _filePath;

        public JsonImmutableConfigReader(string filePath)
        {
            _filePath = filePath;
        }

        public T ReadConfigFile<T>() where T : new()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File {_filePath} doesn't exists");
            }

            if (Path.GetExtension(_filePath) != ".json")
            {
                throw new FormatException($"Provided config file {_filePath} is not json file");
            }

            var notifyConfigJson = File.ReadAllText(_filePath);
            T objectFromJson = JsonConvert.DeserializeObject<T>(notifyConfigJson);
            return objectFromJson;
        }
    }
}
