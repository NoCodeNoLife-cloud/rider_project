using System.Diagnostics;
using Common.Format;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.Trace;

public class MethodBenchmark
{
	private readonly Stopwatch _stopwatch = null!;
	private readonly LogEventLevel _logEventLevel;
	private readonly int _traceLevel;
	private ProfileData _profileData;

	public MethodBenchmark(LogEventLevel logEventLevel, int traceLevel)
	{
		_logEventLevel = logEventLevel;
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch = new Stopwatch();
		_traceLevel = traceLevel;
		Init();
		StartProfile();
	}

	private void Init()
	{
		_profileData.Runtimes = 0;
		_profileData.MaxRuntime = 0;
		_profileData.MinRuntime = int.MaxValue;
		_profileData.AvgRuntime = 0;
	}

	public void StartProfile()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch.Reset();
		_stopwatch.Start();
	}

	public void Record()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch.Stop();
		_profileData.Time = _stopwatch.ElapsedMilliseconds;
		++_profileData.Runtimes;
		_profileData.MaxRuntime = Math.Max(_profileData.Time, _profileData.MaxRuntime);
		_profileData.MinRuntime = Math.Min(_profileData.Time, _profileData.MinRuntime);
		_profileData.AvgRuntime = (_profileData.AvgRuntime * (_profileData.Runtimes - 1) + _profileData.Time) / _profileData.Runtimes;
	}

	public void PrintProfile()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		var callerMethodName = GetCallerMethodName();
		Serilog.Log.Logger.LogColoredWithCallerInfo($"{AnsiEscape.Bold(callerMethodName)} finished in {_profileData.Time} ms. [Max {_profileData.MaxRuntime} ms, Min {_profileData.MinRuntime} ms, Avg {_profileData.AvgRuntime} ms]", _logEventLevel);
	}

	private string GetCallerMethodName()
	{
		var stackTrace = new StackTrace();
		var frame = stackTrace.GetFrame(_traceLevel);
		var method = frame?.GetMethod();
		return method?.Name ?? "Unknown Method";
	}

	private struct ProfileData
	{
		public long Time;
		public long Runtimes;
		public long MaxRuntime;
		public long MinRuntime;
		public long AvgRuntime;
	}
}