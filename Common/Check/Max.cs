namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class Max(double maxValue) : Attribute, IParameterCheck
{
	private double MaxValue { get; } = maxValue;

	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case IComparable comparable:
			{
				if (comparable.CompareTo(MaxValue) > 0)
				{
					throw new ArgumentException($"Value must be less than or equal to {MaxValue}.");
				}

				break;
			}
			default:
				throw new ArgumentException("Value must be a numeric type.");
		}
	}
}