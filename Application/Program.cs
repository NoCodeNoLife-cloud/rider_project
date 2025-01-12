using Common.Configuration;
using Common.FileSystem.Serializer;
using Common.Log.Serilog;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	private const string SerilogSettingPath = "Application/Configuration/Serilog-settings.xml";

	[EnableConfig<LogConfiguration, XmlFileSerializer>(SerilogSettingPath)]
	[EnableTracePerformance(LogEventLevel.Debug)]
	public static void Main(string[] args) { }
}