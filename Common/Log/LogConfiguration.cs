using System.Xml.Serialization;
using Common.Configuration;
using Serilog;
using Serilog.Events;

namespace Common.Log;

[Serializable]
public class LogConfiguration(bool writeToConsole, LogEventLevel logEventLevel, string logFormat) : IConfigurable
{
	[XmlElement] public bool WriteToConsole { get; set; } = writeToConsole;
	[XmlElement] public LogEventLevel LogEventLevel { get; set; } = logEventLevel;
	[XmlElement] public string LogFormat { get; set; } = logFormat;

	public LogConfiguration() : this(false, LogEventLevel.Debug, "") { }

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

	public override string ToString()
	{
		return $"{nameof(WriteToConsole)}: {WriteToConsole}, {nameof(LogEventLevel)}: {LogEventLevel}, {nameof(LogFormat)}: {LogFormat}";
	}
}