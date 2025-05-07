using Common.Log.Serilog;
using Common.Runtime;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class TimeLimitedAttribute(int timeoutThreshold, LogEventLevel logEventLevel = LogEventLevel.Debug, bool profileDetail = false) : MoAttribute
{
    private readonly PerformanceMonitor _benchmark = new(logEventLevel, 3, profileDetail);

    public override void OnEntry(MethodContext context)
    {
        _benchmark.StartProfile();
        base.OnEntry(context);
    }

    public override void OnExit(MethodContext context)
    {
        _benchmark.StopProfileAndRecord();
        if (_benchmark.time > timeoutThreshold) Serilog.Log.Logger.LogWithLevel($"Method {MethodRuntimeKit.GetCallerMethodName()} timeout: {_benchmark.time}ms/{timeoutThreshold}ms", logEventLevel);

        base.OnExit(context);
    }
}