using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

[AttributeUsage(AttributeTargets.Method)]
public class RecordMethodTimeAttribute(LogEventLevel logEventLevel) : MoAttribute
{
	private BenchmarkFunction? _benchmarkFunction;

	public override void OnEntry(MethodContext context)
	{
		_benchmarkFunction = new BenchmarkFunction(logEventLevel, 3);
		base.OnEntry(context);
	}

	public override void OnExit(MethodContext context)
	{
		_benchmarkFunction?.Record();
		base.OnExit(context);
	}
}