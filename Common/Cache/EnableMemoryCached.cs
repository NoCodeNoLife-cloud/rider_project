using System.Reflection;
using System.Runtime.Caching;
using Rougamo;
using Rougamo.Context;

namespace Common.Cache;

public class EnableMemoryCached(int seconds) : MoAttribute
{
    private static readonly MemoryCache MethodRequestCache = MemoryCache.Default;

    public override void OnEntry(MethodContext context)
    {
        var method = context.Method;
        var contextArguments = context.Arguments;
        var cacheKey = GetCacheKey(method, contextArguments);

        var cachedResult = MethodRequestCache.Get(cacheKey);
        if (cachedResult != null) context.ReplaceReturnValue(this, cachedResult);

        base.OnEntry(context);
    }

    public override void OnSuccess(MethodContext context)
    {
        var method = context.Method;
        var contextArguments = context.Arguments;
        var cacheKey = GetCacheKey(method, contextArguments);

        if (context.ReturnValue != null)
        {
            var cachePolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(seconds) };
            MethodRequestCache.Set(cacheKey, context.ReturnValue, cachePolicy);
        }
        else
        {
            throw new ArgumentNullException(nameof(context), "return value cannot be null");
        }

        base.OnSuccess(context);
    }

    private static string GetCacheKey(MethodBase method, object?[] arguments)
    {
        var methodKey = method.DeclaringType?.FullName + "." + method.Name;
        var argumentsKey = string.Join(",", arguments.Select(arg => arg?.ToString() ?? "null"));
        return $"{methodKey}({argumentsKey})";
    }
}