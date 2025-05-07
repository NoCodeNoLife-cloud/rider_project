using Common.Log.Serilog;
using Common.Runtime;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[Flags]
public enum TracedItemEnum
{
    OnEntry,
    OnExit,
    OnException,
    OnSuccess
}

[AttributeUsage(AttributeTargets.Method)]
public class TraceMethodCallAttribute(LogEventLevel logEventLevel = LogEventLevel.Debug, TracedItemEnum calledItemEnum = TracedItemEnum.OnEntry | TracedItemEnum.OnExit) : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        if (calledItemEnum.HasFlag(TracedItemEnum.OnEntry) && Serilog.Log.Logger.IsEnabled(logEventLevel)) Serilog.Log.Logger.LogWithLevel($"{MethodRuntimeKit.GetCallerMethodName()} entry", logEventLevel);

        base.OnEntry(context);
    }

    public override void OnException(MethodContext context)
    {
        if (calledItemEnum.HasFlag(TracedItemEnum.OnException) && Serilog.Log.Logger.IsEnabled(logEventLevel)) Serilog.Log.Logger.LogWithLevel($"{MethodRuntimeKit.GetCallerMethodName()} throw {context.Exception?.Message}", logEventLevel);

        base.OnException(context);
    }

    public override void OnSuccess(MethodContext context)
    {
        if (calledItemEnum.HasFlag(TracedItemEnum.OnSuccess) && Serilog.Log.Logger.IsEnabled(logEventLevel)) Serilog.Log.Logger.LogWithLevel($"{MethodRuntimeKit.GetCallerMethodName()} success", logEventLevel);

        base.OnSuccess(context);
    }

    public override void OnExit(MethodContext context)
    {
        if (calledItemEnum.HasFlag(TracedItemEnum.OnExit) && Serilog.Log.Logger.IsEnabled(logEventLevel)) Serilog.Log.Logger.LogWithLevel($"{MethodRuntimeKit.GetCallerMethodName()} exit", logEventLevel);

        base.OnExit(context);
    }
}