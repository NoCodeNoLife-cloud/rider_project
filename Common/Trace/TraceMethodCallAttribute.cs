﻿using System.Diagnostics;
using Common.Log.Serilog;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class TraceMethodCallAttribute(LogEventLevel logEventLevel, TracedItemEnum calledItemEnum) : MoAttribute
{
	public override void OnEntry(MethodContext context)
	{
		if (calledItemEnum.HasFlag(TracedItemEnum.OnEntry) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogColoredWithCallerInfo($"{GetCallerMethodName()} start", logEventLevel);
		}

		base.OnEntry(context);
	}

	public override void OnException(MethodContext context)
	{
		if (calledItemEnum.HasFlag(TracedItemEnum.OnException) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogColoredWithCallerInfo($"{GetCallerMethodName()} throw {context.Exception?.Message}", logEventLevel);
		}

		base.OnException(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		if (calledItemEnum.HasFlag(TracedItemEnum.OnSuccess) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogColoredWithCallerInfo($"{GetCallerMethodName()} success", logEventLevel);
		}

		base.OnSuccess(context);
	}

	public override void OnExit(MethodContext context)
	{
		if (calledItemEnum.HasFlag(TracedItemEnum.OnExit) && Serilog.Log.Logger.IsEnabled(logEventLevel))
		{
			Serilog.Log.Logger.LogColoredWithCallerInfo($"{GetCallerMethodName()} end", logEventLevel);
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