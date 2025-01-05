using System.Diagnostics;
using Common.Log;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class EnableTraceCallAttribute(LogEventLevel logEventLevel, TraceCalledItem calledItem) : MoAttribute
{
	public override void OnEntry(MethodContext context)
	{
		if (calledItem.HasFlag(TraceCalledItem.OnEntry) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} start", logEventLevel);
		}

		base.OnEntry(context);
	}

	public override void OnException(MethodContext context)
	{
		if (calledItem.HasFlag(TraceCalledItem.OnException) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} throw {context.Exception?.Message}", logEventLevel);
		}

		base.OnException(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		if (calledItem.HasFlag(TraceCalledItem.OnSuccess) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} success", logEventLevel);
		}

		base.OnSuccess(context);
	}

	public override void OnExit(MethodContext context)
	{
		if (calledItem.HasFlag(TraceCalledItem.OnExit) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} end", logEventLevel);
		}

		base.OnExit(context);
	}

	private static string GetCallerMethodName()
	{
		var stackTrace = new StackTrace();
		var frame = stackTrace.GetFrame(2);
		var method = frame?.GetMethod();
		return method?.Name ?? "Unknown Method";
	}
}