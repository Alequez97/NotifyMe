namespace CommonUtils.Interfaces
{
    public interface IConfigReader
	{
		T ReadConfigFile<T>(string path) where T : new();
	}
}
