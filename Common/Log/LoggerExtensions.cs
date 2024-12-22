using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Common.Log;

public static class LoggerExtensions
{
	public static void LogWithCallerInfo(
		this ILogger logger,
		string message,
		LogEventLevel level = LogEventLevel.Information,
		[CallerMemberName] string callerMemberName = "",
		[CallerFilePath] string callerFilePath = "",
		[CallerLineNumber] int callerLineNumber = 0)
	{
		var callerFileUrl = Path.GetFileName(callerFilePath);
		var callerThreadName = Thread.CurrentThread.Name ?? $"Thread-{Environment.CurrentManagedThreadId}";
		ILogEventEnricher enrich = new DynamicCallerInfoEnrich(callerThreadName, callerMemberName, callerFileUrl, callerLineNumber);
		logger.ForContext(enrich).Write(level, message);
	}
}