using Common.Configuration;
using Common.Log;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	static Program()
	{
		List<IConfigurable> configurableList = [];
		ConfigurationManager.LoadConfiguration<LogConfiguration>(configurableList, typeof(LogConfiguration));
		ConfigurationManager.ConfigureAll(configurableList);

		Serilog.Log.Logger.LogWithCallerInfo("finished configuration", LogEventLevel.Debug);
	}

	[EnableTracePerformance(LogEventLevel.Debug)]
	public static void Main(string[] args) { }
}