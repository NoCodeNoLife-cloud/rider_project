using System.Reflection;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class RecordMethodTimeAttribute(LogEventLevel logEventLevel) : MoAttribute
{
	private static readonly Dictionary<string, MethodBenchmark> Benchmarks = [];
	private const int TraceLevel = 3;

	public override void OnEntry(MethodContext context)
	{
		var key = CalcKey(context.Method, context.Arguments);
		if (!Benchmarks.ContainsKey(key))
		{
			Benchmarks[key] = new MethodBenchmark(logEventLevel, TraceLevel);
		}

		Benchmarks[key].StartProfile();
		base.OnEntry(context);
	}

	public override void OnExit(MethodContext context)
	{
		var benchmark = Benchmarks[CalcKey(context.Method, context.Arguments)];
		benchmark.Record();
		benchmark.PrintProfile();
		base.OnExit(context);
	}

	private static string CalcKey(MethodBase method, object?[] arguments)
	{
		var methodKey = method.DeclaringType?.FullName + "." + method.Name;
		var argumentsKey = string.Join(",", arguments.Select(arg => arg?.ToString() ?? "null"));
		return $"{methodKey}({argumentsKey})";
	}
}