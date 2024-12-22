using System.Diagnostics;
using Common.Log;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

public class EnableTraceCallAttribute(LogEventLevel logEventLevel, TraceCallItem callItem) : MoAttribute
{
	public override void OnEntry(MethodContext context)
	{
		if (callItem.HasFlag(TraceCallItem.OnEntry) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} start", logEventLevel);
		}

		base.OnEntry(context);
	}

	public override void OnException(MethodContext context)
	{
		if (callItem.HasFlag(TraceCallItem.OnException) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} throw {context.Exception?.Message}", logEventLevel);
		}

		base.OnException(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		if (callItem.HasFlag(TraceCallItem.OnSuccess) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogWithCallerInfo($"{GetCallerMethodName()} success", logEventLevel);
		}

		base.OnSuccess(context);
	}

	public override void OnExit(MethodContext context)
	{
		if (callItem.HasFlag(TraceCallItem.OnExit) && Serilog.Log.Logger.IsEnabled(logEventLevel))
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