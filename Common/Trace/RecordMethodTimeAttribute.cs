using System.Reflection;
using Common.Runtime;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class RecordMethodTimeAttribute(LogEventLevel logEventLevel = LogEventLevel.Debug, bool profileDetail = false) : MoAttribute
{
	private static readonly Dictionary<string, PerformanceMonitor> Benchmarks = [];
	private const int TraceLevel = 3;

	public override void OnEntry(MethodContext context)
	{
		var key = MethodRuntimeKit.GenerateFunctionBasedUniqueId(context.Method, context.Arguments);
		if (!Benchmarks.TryGetValue(key, out var value))
		{
			value = new PerformanceMonitor(logEventLevel, TraceLevel, profileDetail);
			Benchmarks[key] = value;
		}

		value.StartProfile();
		base.OnEntry(context);
	}

	public override void OnExit(MethodContext context)
	{
		var benchmark = Benchmarks[MethodRuntimeKit.GenerateFunctionBasedUniqueId(context.Method, context.Arguments)];
		benchmark.StopProfileAndRecord();
		benchmark.PrintProfile();
		base.OnExit(context);
	}
}