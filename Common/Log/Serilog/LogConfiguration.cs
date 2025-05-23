﻿using Common.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Common.Log.Serilog;

[Serializable]
public class LogConfiguration(bool writeToConsole, LogEventLevel logEventLevel, string logFormat, string ansiConsoleTheme) : IConfigurable
{
    public LogConfiguration() : this(false, LogEventLevel.Debug, "", "Code")
    {
    }

    public bool writeToConsole { get; set; } = writeToConsole;
    public LogEventLevel logEventLevel { get; set; } = logEventLevel;
    public string logFormat { get; set; } = logFormat;
    public string theme { get; set; } = ansiConsoleTheme;

    public void Configure()
    {
        var loggerConfiguration = new LoggerConfiguration();

        SetLogOutput(loggerConfiguration);
        SetLogEventLevel(loggerConfiguration);

        global::Serilog.Log.Logger = loggerConfiguration.CreateLogger();
    }

    private AnsiConsoleTheme GetTheme()
    {
        return theme switch
        {
            "Code" => AnsiConsoleTheme.Code,
            "Grayscale" => AnsiConsoleTheme.Grayscale,
            "Literate" => AnsiConsoleTheme.Literate,
            "Sixteen" => AnsiConsoleTheme.Sixteen,
            _ => AnsiConsoleTheme.Code
        };
    }

    private void SetLogOutput(LoggerConfiguration loggerConfiguration)
    {
        if (writeToConsole) loggerConfiguration.WriteTo.Console(theme: GetTheme(), outputTemplate: logFormat);
    }

    private void SetLogEventLevel(LoggerConfiguration loggerConfiguration)
    {
        switch (logEventLevel)
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
                throw new ArgumentOutOfRangeException(nameof(loggerConfiguration), logEventLevel, null);
        }
    }
}