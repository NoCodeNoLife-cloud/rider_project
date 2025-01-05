using Common.FileSystem.Serializer;
using Common.Log;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Configuration;

[AttributeUsage(AttributeTargets.Method)]
public class EnableConfigAttribute<T>(string? settingFilePath) : MoAttribute where T : IConfigurable, new()
{
	private const string RootPath = "../../../../";

	public override void OnEntry(MethodContext context)
	{
		IConfigurable? config;
		if (settingFilePath == null)
		{
			config = new T();
		}
		else
		{
			var configurationPath = RootPath + settingFilePath;
			config = JsonFileSerializer.DeserializeFromFile<T>(configurationPath);
		}

		config.Configure();
		base.OnEntry(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		Serilog.Log.Logger.LogWithCallerInfo($"finished configure {typeof(T)} from {settingFilePath}", LogEventLevel.Debug);
		base.OnSuccess(context);
	}
}