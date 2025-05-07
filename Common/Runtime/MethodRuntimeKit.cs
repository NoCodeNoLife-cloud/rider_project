using System.Diagnostics;
using System.Reflection;

namespace Common.Runtime;

public static class MethodRuntimeKit
{
    public static string GetCallerMethodName()
    {
        var stackTrace = new StackTrace();
        var frame = stackTrace.GetFrame(2);
        var method = frame?.GetMethod();
        return method?.Name ?? "Unknown Method";
    }

    public static string GenerateFunctionBasedUniqueId(MethodBase method, object?[] arguments)
    {
        var methodKey = method.DeclaringType?.FullName + "." + method.Name;
        var argumentsKey = string.Join(",", arguments.Select(arg => arg?.ToString() ?? "null"));
        return $"{methodKey}({argumentsKey})";
    }
}