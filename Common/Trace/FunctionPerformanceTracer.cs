using System.Diagnostics;
using Common.Log;
using Serilog.Events;

namespace Common.Trace;

public class FunctionPerformanceTracer : IDisposable
{
	private readonly Stopwatch? _stopwatch;
	private bool _disposed;
	private readonly LogEventLevel _logEventLevel;
	private readonly int _traceLevel;

	public FunctionPerformanceTracer(LogEventLevel logEventLevel, int traceLevel = 3)
	{
		_logEventLevel = logEventLevel;
		if (Serilog.Log.IsEnabled(_logEventLevel))
		{
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
			_traceLevel = traceLevel;
		}

		_disposed = false;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed) return;
		if (disposing)
		{
			if (Serilog.Log.IsEnabled(_logEventLevel))
			{
				_stopwatch?.Stop();
				var callerMethodName = GetCallerMethodName();
				Serilog.Log.Logger.LogWithCallerInfo($"{callerMethodName} finished in {_stopwatch?.ElapsedMilliseconds} ms.", _logEventLevel);
			}
		}

		_disposed = true;
	}

	private string GetCallerMethodName()
	{
		var stackTrace = new StackTrace();
		var frame = stackTrace.GetFrame(_traceLevel);
		var method = frame?.GetMethod();
		return method?.Name ?? "Unknown Method";
	}

	~FunctionPerformanceTracer()
	{
		Dispose(false);
	}
}