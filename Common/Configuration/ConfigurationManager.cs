using Common.FileSystem.Serializer;
using Common.Log;

namespace Common.Configuration;

public static class ConfigurationManager
{
	private const string RootPath = "../../../../";

	private static readonly Dictionary<Type, string> ConfigurationPaths = new()
	{
		{ typeof(LogConfiguration), RootPath + "Application/Configuration/Serilog-settings.json" },
	};

	public static void LoadConfiguration<T>(List<IConfigurable> configurableList, Type configurationType) where T : IConfigurable
	{
		var configurationPath = ConfigurationPaths[configurationType];
		configurableList.Add(JsonFileSerializer.DeserializeFromFile<T>(configurationPath));
	}

	public static void ConfigureAll(List<IConfigurable> configurableList)
	{
		foreach (var configurable in configurableList)
		{
			configurable.Configure();
		}
	}
}