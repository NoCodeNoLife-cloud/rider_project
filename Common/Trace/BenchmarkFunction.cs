using System.Diagnostics;
using Common.Format;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.Trace;

public class BenchmarkFunction
{
	private readonly Stopwatch? _stopwatch;
	private readonly LogEventLevel _logEventLevel;
	private readonly int _traceLevel;

	public BenchmarkFunction(LogEventLevel logEventLevel, int traceLevel)
	{
		_logEventLevel = logEventLevel;
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch = new Stopwatch();
		_stopwatch.Start();
		_traceLevel = traceLevel;
	}

	public void Record()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch?.Stop();
		var callerMethodName = GetCallerMethodName();
		Serilog.Log.Logger.LogColoredWithCallerInfo($"{AnsiEscape.Bold(callerMethodName)} finished in {_stopwatch?.ElapsedMilliseconds} ms.", _logEventLevel);
	}

	private string GetCallerMethodName()
	{
		var stackTrace = new StackTrace();
		var frame = stackTrace.GetFrame(_traceLevel);
		var method = frame?.GetMethod();
		return method?.Name ?? "Unknown Method";
	}
}