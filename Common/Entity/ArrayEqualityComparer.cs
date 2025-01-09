namespace Common.Entity;

public class ArrayEqualityComparer: IEqualityComparer<object?[]>
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