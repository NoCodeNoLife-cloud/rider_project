using Common.FileSystem.Serializer;
using Common.Log.Serilog;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Configuration;

[AttributeUsage(AttributeTargets.Method)]
public class ConfigServiceAttribute<TV, TS>(string? settingFilePath, LogEventLevel logEventLevel) : MoAttribute where TV : IConfigurable, new() where TS : IFileSerializable
{
	private const string RootPath = "../../../../";

	public override void OnEntry(MethodContext context)
	{
		IConfigurable? config;
		if (settingFilePath == null)
		{
			config = new TV();
		}
		else
		{
			var configurationPath = RootPath + settingFilePath;
			config = TS.DeserializeFromFile<TV>(configurationPath);
		}

		config?.Configure();
		Serilog.Log.Logger.LogWithLevel($"finished configure {typeof(TV)} from {settingFilePath}", logEventLevel);
		base.OnEntry(context);
	}
}