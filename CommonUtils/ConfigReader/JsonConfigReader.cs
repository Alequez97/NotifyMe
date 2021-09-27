using Newtonsoft.Json;
using System;
using System.IO;

namespace CommonUtils.ConfigReader
{
    public class JsonConfigReader : IConfigReader
    {
        public T ReadConfigFile<T>(string path) where T : new()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} doesn't exists");
            }

            if (Path.GetExtension(path) != ".json")
            {
                throw new FormatException($"Provided config file {path} is not json file");
            }

            var notifyConfigJson = File.ReadAllText(path);
            T objectFromJson = JsonConvert.DeserializeObject<T>(notifyConfigJson);
            return objectFromJson;
        }
    }
}
