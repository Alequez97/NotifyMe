namespace CommonUtils.Interfaces
{
    public interface IImmutableConfigReader
	{
		T ReadConfigFile<T>() where T : new();
	}
}
