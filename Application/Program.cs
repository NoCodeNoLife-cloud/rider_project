using Common.Configuration;
using Common.Exception;
using Common.FileSystem.Serializer;
using Common.Log.Serilog;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	private const string SerilogSettingPath = "Application/Configuration/Serilog-settings.xml";

	[ConfigService<LogConfiguration, XmlFileSerializer>(SerilogSettingPath)]
	[TracePerformance(LogEventLevel.Debug)]
	[HandleException]
	public static void Main(string[] args) { }
}