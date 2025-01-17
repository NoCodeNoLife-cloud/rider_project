using System.Diagnostics;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.Trace;

public class PerformanceMonitor
{
	private readonly Stopwatch _stopwatch = null!;
	private readonly LogEventLevel _logEventLevel;
	private readonly int _traceLevel;
	private ProfileData _profileData;
	private readonly bool _enableProfileDetail;

	public long time { get; private set; }

	public PerformanceMonitor(LogEventLevel logEventLevel, int traceLevel, bool enableProfileDetail)
	{
		_logEventLevel = logEventLevel;
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch = new Stopwatch();
		_traceLevel = traceLevel;
		_enableProfileDetail = enableProfileDetail;
		if (_enableProfileDetail)
		{
			Init();
		}

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

	public void StopProfileAndRecord()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch.Stop();
		time = _stopwatch.ElapsedMilliseconds;
		if (!_enableProfileDetail) return;
		++_profileData.Runtimes;
		_profileData.MaxRuntime = Math.Max(time, _profileData.MaxRuntime);
		_profileData.MinRuntime = Math.Min(time, _profileData.MinRuntime);
		_profileData.AvgRuntime = (_profileData.AvgRuntime * (_profileData.Runtimes - 1) + time) / _profileData.Runtimes;
	}

	public void PrintProfile()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		var callerMethodName = GetCallerMethodName();
		Serilog.Log.Logger.LogWithLevel($"{callerMethodName} finished in {time} ms. " + (_enableProfileDetail ? $"[Max {_profileData.MaxRuntime} ms, Min {_profileData.MinRuntime} ms, Avg {_profileData.AvgRuntime} ms]" : string.Empty), _logEventLevel);
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
		public long Runtimes;
		public long MaxRuntime;
		public long MinRuntime;
		public long AvgRuntime;
	}
}