using System.Diagnostics;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.Trace;

public class MethodBenchmark
{
	private readonly Stopwatch _stopwatch = null!;
	private readonly LogEventLevel _logEventLevel;
	private readonly int _traceLevel;
	private long _time;
	private ProfileData _profileData;
	private readonly bool _profileDetail;

	public MethodBenchmark(LogEventLevel logEventLevel, int traceLevel, bool profileDetail)
	{
		_logEventLevel = logEventLevel;
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch = new Stopwatch();
		_traceLevel = traceLevel;
		_profileDetail = profileDetail;
		if (_profileDetail)
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

	public void Record()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		_stopwatch.Stop();
		_time = _stopwatch.ElapsedMilliseconds;
		if (_profileDetail)
		{
			++_profileData.Runtimes;
			_profileData.MaxRuntime = Math.Max(_time, _profileData.MaxRuntime);
			_profileData.MinRuntime = Math.Min(_time, _profileData.MinRuntime);
			_profileData.AvgRuntime = (_profileData.AvgRuntime * (_profileData.Runtimes - 1) + _time) / _profileData.Runtimes;
		}
	}

	public void PrintProfile()
	{
		if (!Serilog.Log.IsEnabled(_logEventLevel)) return;
		var callerMethodName = GetCallerMethodName();
		Serilog.Log.Logger.LogWithLevel($"{callerMethodName} finished in {_time} ms. " + ((_profileDetail) ? $"[Max {_profileData.MaxRuntime} ms, Min {_profileData.MinRuntime} ms, Avg {_profileData.AvgRuntime} ms]" : string.Empty), _logEventLevel);
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