using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Common.Log.Serilog;

public static class LoggerExtensions
{
	public static void LogWithLevel(
		this ILogger logger,
		string message,
		LogEventLevel level = LogEventLevel.Information,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		CustomLog(logger, message, level, callerMemberName, callerFilePath, callerLineNumber);
	}

	public static void Debug(
		this ILogger logger,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		CustomLog(logger, message, LogEventLevel.Debug, callerMemberName, callerFilePath, callerLineNumber);
	}

	public static void ConditionalDebug(
		this ILogger logger,
		bool condition,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		if (condition)
		{
			CustomLog(logger, message, LogEventLevel.Debug, callerMemberName, callerFilePath, callerLineNumber);
		}
	}

	public static void Info(
		this ILogger logger,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		CustomLog(logger, message, LogEventLevel.Information, callerMemberName, callerFilePath, callerLineNumber);
	}

	public static void ConditionalInfo(
		this ILogger logger,
		bool condition,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		if (condition)
		{
			CustomLog(logger, message, LogEventLevel.Information, callerMemberName, callerFilePath, callerLineNumber);
		}
	}

	public static void Error(
		this ILogger logger,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		CustomLog(logger, message, LogEventLevel.Error, callerMemberName, callerFilePath, callerLineNumber);
	}

	public static void ConditionalError(
		this ILogger logger,
		bool condition,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		if (condition)
		{
			CustomLog(logger, message, LogEventLevel.Error, callerMemberName, callerFilePath, callerLineNumber);
		}
	}

	public static void Fatal(
		this ILogger logger,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		CustomLog(logger, message, LogEventLevel.Fatal, callerMemberName, callerFilePath, callerLineNumber);
	}

	public static void ConditionalFatal(
		this ILogger logger,
		bool condition,
		string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		if (condition)
		{
			CustomLog(logger, message, LogEventLevel.Fatal, callerMemberName, callerFilePath, callerLineNumber);
		}
	}

	private static void CustomLog(
		this ILogger logger,
		string message,
		LogEventLevel level = LogEventLevel.Information,
		string callerMemberName = "",
		string callerFilePath = "",
		int callerLineNumber = 0)
	{
		var callerFileUrl = Path.GetFileName(callerFilePath);
		var callerThreadName = Thread.CurrentThread.Name ?? $"Thread-{Environment.CurrentManagedThreadId}";
		var enrichedMessage = EnrichMessageWithColor(message, level);
		ILogEventEnricher enrich = new DynamicCallerInfoEnrich(callerThreadName, callerMemberName, callerFileUrl, callerLineNumber);
		logger.ForContext(enrich).Write(level, enrichedMessage);
		if (level == LogEventLevel.Fatal)
		{
			throw new System.Exception($"Fatal error: {message}");
		}
	}

	private static string EnrichMessageWithColor(string message, LogEventLevel level)
	{
		var colorCode = level switch
		{
			LogEventLevel.Verbose => "\e[34m",
			LogEventLevel.Debug => "\e[90m",
			LogEventLevel.Information => "\e[32m",
			LogEventLevel.Warning => "\e[33m",
			LogEventLevel.Error => "\e[31m",
			LogEventLevel.Fatal => "\e[31m",
			_ => "\e[37m",
		};

		return $" ==> {colorCode}{message}\e[0m";
	}
}