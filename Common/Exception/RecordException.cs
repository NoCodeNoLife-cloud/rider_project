using System.Text.RegularExpressions;
using Common.Log.Serilog;
using Rougamo;
using Rougamo.Context;
using Serilog.Events;

namespace Common.Exception;

[AttributeUsage(AttributeTargets.Method)]
public partial class RecordException(bool ignore = false) : MoAttribute
{
	public override void OnException(MethodContext context)
	{
		if (ignore)
		{
			Serilog.Log.Logger.LogColoredWithCallerInfo($"ignored exception: {FormatExceptionStackTrace(context.Exception!)}", LogEventLevel.Warning);
			context.HandledException(this, context.ReturnValue!);
			return;
		}

		Serilog.Log.Logger.LogColoredWithCallerInfo($"throw exception: {FormatExceptionStackTrace(context.Exception!)}", LogEventLevel.Error);
	}

	[GeneratedRegex(@"\s+at\s+(.*)\s+in\s+(.+):line\s+(\d+)")]
	private static partial Regex MyRegex();

	private static string FormatExceptionStackTrace(System.Exception ex)
	{
		var stackTrace = ex.StackTrace;
		if (stackTrace == null)
		{
			return ex.ToString();
		}

		var lines = stackTrace.Split('\n');
		var regex = MyRegex();

		for (var i = 0; i < lines.Length; i++)
		{
			lines[i] = regex.Replace(lines[i], match =>
			{
				var method = match.Groups[1].Value;
				var filePath = match.Groups[2].Value;
				var lineNumber = match.Groups[3].Value;

				var fileName = Path.GetFileName(filePath);
				return $"   at {method} in {fileName}:{lineNumber}";
			});
		}

		return $"{ex.GetType()}: {ex.Message}\n{string.Join("\n", lines)}";
	}
}