using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Trace;

public class EnableTracePerformanceAttribute(LogEventLevel logEventLevel) : MoAttribute
{
	private FunctionPerformanceTracer? _functionPerformanceTracker;

	public override void OnEntry(MethodContext context)
	{
		_functionPerformanceTracker = new FunctionPerformanceTracer(logEventLevel, 4);
		base.OnEntry(context);
	}

	public override void OnExit(MethodContext context)
	{
		_functionPerformanceTracker?.Dispose();
		base.OnExit(context);
	}
}