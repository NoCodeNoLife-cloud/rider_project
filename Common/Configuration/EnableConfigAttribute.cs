using Common.FileSystem.Serializer;
using Common.Log;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Configuration;

[AttributeUsage(AttributeTargets.Method)]
public class EnableConfigAttribute<TV, TS>(string? settingFilePath) : MoAttribute where TV : IConfigurable, new() where TS : IFileSerializable
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
		base.OnEntry(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		Serilog.Log.Logger.LogWithCallerInfo($"finished configure {typeof(TV)} from {settingFilePath}", LogEventLevel.Debug);
		base.OnSuccess(context);
	}
}