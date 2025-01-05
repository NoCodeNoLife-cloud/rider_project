using Common.Configuration;
using Common.Log;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	[EnableConfig<LogConfiguration>("Application/Configuration/Serilog-settings.json")]
	[EnableTracePerformance(LogEventLevel.Debug)]
	public static void Main(string[] args) { }
}