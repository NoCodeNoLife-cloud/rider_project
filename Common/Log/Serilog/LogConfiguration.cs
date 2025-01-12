using System.Xml.Serialization;
using Common.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Common.Log.Serilog;

[Serializable]
public class LogConfiguration(bool writeToConsole, LogEventLevel logEventLevel, string logFormat, string ansiConsoleTheme) : IConfigurable
{
	[XmlElement] public bool WriteToConsole { get; set; } = writeToConsole;
	[XmlElement] public LogEventLevel LogEventLevel { get; set; } = logEventLevel;
	[XmlElement] public string LogFormat { get; set; } = logFormat;
	[XmlElement] public string Theme { get; set; } = ansiConsoleTheme;

	public LogConfiguration() : this(false, LogEventLevel.Debug, "", "Code") { }

	private AnsiConsoleTheme GetTheme()
	{
		return Theme switch
		{
			"Code" => AnsiConsoleTheme.Code,
			"Grayscale" => AnsiConsoleTheme.Grayscale,
			"Literate" => AnsiConsoleTheme.Literate,
			"Sixteen" => AnsiConsoleTheme.Sixteen,
			_ => AnsiConsoleTheme.Code
		};
	}

	public void Configure()
	{
		var loggerConfiguration = new LoggerConfiguration();

		SetLogOutput(loggerConfiguration);
		SetLogEventLevel(loggerConfiguration);

		global::Serilog.Log.Logger = loggerConfiguration.CreateLogger();
	}

	private void SetLogOutput(LoggerConfiguration loggerConfiguration)
	{
		if (WriteToConsole)
		{
			loggerConfiguration.WriteTo.Console(theme: GetTheme(), outputTemplate: LogFormat);
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