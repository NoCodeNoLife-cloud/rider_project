using System.Reflection;
using System.Runtime.Caching;
using Rougamo;
using Rougamo.Context;

namespace Common.Cache;

[AttributeUsage(AttributeTargets.Method)]
public class EnableIdempotencyAttribute : MoAttribute
{
    private static readonly MemoryCache MethodRequestToken = MemoryCache.Default;

    public override void OnEntry(MethodContext context)
    {
        var method = context.Method;
        var contextArguments = context.Arguments;
        var cacheKey = GetCacheKey(method, contextArguments);

        var cachedResult = MethodRequestToken.Get(cacheKey);
        if (cachedResult != null) context.ReplaceReturnValue(this, cachedResult);

        base.OnEntry(context);
    }

    public override void OnSuccess(MethodContext context)
    {
        var method = context.Method;
        var contextArguments = context.Arguments;
        var cacheKey = GetCacheKey(method, contextArguments);

        if (context.ReturnValue != null) MethodRequestToken.Set(cacheKey, context.ReturnValue, null);

        base.OnSuccess(context);
    }

    private static string GetCacheKey(MethodBase method, object?[] arguments)
    {
        var methodKey = method.DeclaringType?.FullName + "." + method.Name;
        var argumentsKey = string.Join(",", arguments.Select(arg => arg?.ToString() ?? "null"));
        return $"{methodKey}({argumentsKey})";
    }
}