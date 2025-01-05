using Common.Configuration;
using Common.FileSystem.Serializer;
using Common.Log;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	[EnableConfig<LogConfiguration, XmlFileSerializer>("Application/Configuration/Serilog-settings.xml")]
	[EnableTracePerformance(LogEventLevel.Debug)]
	public static void Main(string[] args) { }
}