using System.Text.RegularExpressions;
using Common.Log.Serilog;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Exception;

[AttributeUsage(AttributeTargets.Method)]
public partial class HandleException : MoAttribute
{
	public override void OnException(MethodContext context)
	{
		Serilog.Log.Logger.LogColoredWithCallerInfo($"{FormatException(context.Exception!)}", LogEventLevel.Error);
		context.HandledException(this, context.ReturnValue!);
	}

	[GeneratedRegex(@"at\s+.+\s+in\s+.*\\([^\s]+):line\s+(\d+)")]
	private static partial Regex MyRegex();

	private static string FormatException(System.Exception ex)
	{
		var exceptionType = ex.GetType().ToString();
		var message = ex.Message;
		var stackTrace = ex.StackTrace;

		if (stackTrace == null) return string.Empty;

		var match = MyRegex().Match(stackTrace);
		if (!match.Success)
		{
			return string.Empty;
		}

		var fileName = match.Groups[1].Value;
		var lineNumber = match.Groups[2].Value;

		return $"{exceptionType} in {fileName}:{lineNumber} => {message} ";
	}
}