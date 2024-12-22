namespace common.Configuration;

public static class ConfigurationManager
{
	public static void Configure(List<IConfigurable> configurableList)
	{
		foreach (var configurable in configurableList)
		{
			configurable.Configure();
		}
	}
}