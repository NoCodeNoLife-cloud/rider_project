using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Common.Log.Serilog;

public static class LoggerExtensions
{
	public static void LogColoredWithCallerInfo(
		this ILogger logger,
		string message,
		LogEventLevel level = LogEventLevel.Information,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		var callerFileUrl = Path.GetFileName(callerFilePath);
		var callerThreadName = Thread.CurrentThread.Name ?? $"Thread-{Environment.CurrentManagedThreadId}";
		var enrichedMessage = EnrichMessageWithColor(message, level);
		ILogEventEnricher enrich = new DynamicCallerInfoEnrich(callerThreadName, callerMemberName, callerFileUrl, callerLineNumber);
		logger.ForContext(enrich).Write(level, enrichedMessage);
	}

	private static string EnrichMessageWithColor(string message, LogEventLevel level)
	{
		var colorCode = level switch
		{
			LogEventLevel.Verbose => "\x1b[34m",
			LogEventLevel.Debug => "\x1b[90m",
			LogEventLevel.Information => "\x1b[32m",
			LogEventLevel.Warning => "\x1b[33m",
			LogEventLevel.Error => "\x1b[31m",
			LogEventLevel.Fatal => "\x1b[31m",
			_ => "\x1b[37m",
		};

		return $"{colorCode}{message}\x1b[0m";
	}
}