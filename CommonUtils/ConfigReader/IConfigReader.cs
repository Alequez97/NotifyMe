namespace CommonUtils.ConfigReader
{
    public interface IConfigReader
	{
		T ReadConfigFile<T>(string path) where T : new();
	}
}
