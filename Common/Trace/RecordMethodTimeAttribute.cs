using System.Reflection;
using Common.Runtime;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class RecordMethodTimeAttribute(LogEventLevel logEventLevel, bool profileDetail = false) : MoAttribute
{
	private static readonly Dictionary<string, MethodBenchmark> Benchmarks = [];
	private const int TraceLevel = 3;

	public override void OnEntry(MethodContext context)
	{
		var key = MethodRuntime.GenerateFunctionBasedUniqueId(context.Method, context.Arguments);
		if (!Benchmarks.TryGetValue(key, out var value))
		{
			value = new MethodBenchmark(logEventLevel, TraceLevel, profileDetail);
			Benchmarks[key] = value;
		}

		value.StartProfile();
		base.OnEntry(context);
	}

	public override void OnExit(MethodContext context)
	{
		var benchmark = Benchmarks[MethodRuntime.GenerateFunctionBasedUniqueId(context.Method, context.Arguments)];
		benchmark.StopProfileAndRecord();
		benchmark.PrintProfile();
		base.OnExit(context);
	}
}