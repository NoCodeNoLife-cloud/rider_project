using System.Collections.Concurrent;
using System.Reflection;
using Rougamo;
using Rougamo.Context;

namespace Common.Cache;

public class EnableCachedAttribute : MoAttribute
{
	private static readonly ConcurrentDictionary<MethodBase, ConcurrentDictionary<object?[], object?>> MethodCache = new();

	public override void OnEntry(MethodContext context)
	{
		var method = context.Method;
		if (!MethodCache.TryGetValue(method, out var cache))
		{
			cache = new ConcurrentDictionary<object?[], object?>(new ArrayEqualityComparer());
			MethodCache.TryAdd(method, cache);
		}

		var contextArguments = context.Arguments;
		if (cache.TryGetValue(contextArguments, out var cachedResult))
		{
			context.ReplaceReturnValue(this, cachedResult!);
		}

		base.OnEntry(context);
	}

	public override void OnSuccess(MethodContext context)
	{
		var method = MethodCache[context.Method];
		method[context.Arguments] = context.ReturnValue;
		base.OnSuccess(context);
	}

	private class ArrayEqualityComparer : IEqualityComparer<object?[]>
	{
		public bool Equals(object?[]? x, object?[]? y)
		{
			if (x == null || y == null) return x == y;
			return x.Length == y.Length && x.SequenceEqual(y);
		}

		public int GetHashCode(object?[] obj)
		{
			unchecked
			{
				return obj.Aggregate(17, (current, item) => current * 31 + (item?.GetHashCode() ?? 0));
			}
		}
	}
}