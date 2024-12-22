using System.Text.Json.Serialization;
using Common.Configuration;
using Serilog;
using Serilog.Events;

namespace Common.Log;

public class LogConfiguration(bool writeToConsole, LogEventLevel logEventLevel, string logFormat) : IConfigurable
{
	[JsonInclude] private bool WriteToConsole { get; } = writeToConsole;
	[JsonInclude] private LogEventLevel LogEventLevel { get; } = logEventLevel;
	[JsonInclude] private string LogFormat { get; } = logFormat;

	public void Configure()
	{
		var loggerConfiguration = new LoggerConfiguration();

		SetLogOutput(loggerConfiguration);
		SetLogEventLevel(loggerConfiguration);

		Serilog.Log.Logger = loggerConfiguration.CreateLogger();
	}

	private void SetLogOutput(LoggerConfiguration loggerConfiguration)
	{
		if (WriteToConsole)
		{
			loggerConfiguration.WriteTo.Console(outputTemplate: LogFormat);
		}
	}

	private void SetLogEventLevel(LoggerConfiguration loggerConfiguration)
	{
		switch (LogEventLevel)
		{
			case LogEventLevel.Debug:
				loggerConfiguration.MinimumLevel.Debug();
				break;
			case LogEventLevel.Error:
				loggerConfiguration.MinimumLevel.Error();
				break;
			case LogEventLevel.Information:
				loggerConfiguration.MinimumLevel.Information();
				break;
			case LogEventLevel.Warning:
				loggerConfiguration.MinimumLevel.Warning();
				break;
			case LogEventLevel.Fatal:
				loggerConfiguration.MinimumLevel.Fatal();
				break;
			case LogEventLevel.Verbose:
				loggerConfiguration.MinimumLevel.Verbose();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(loggerConfiguration), LogEventLevel, null);
		}
	}
}