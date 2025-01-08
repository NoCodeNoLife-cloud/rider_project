namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class MaxAttribute(double max) : Attribute, IParameterCheck
{
	private double Max { get; } = max;

	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case IComparable comparable:
			{
				if (comparable.CompareTo(Max) > 0)
				{
					throw new ArgumentException($"Value must be less than or equal to {Max}.");
				}

				break;
			}
			default:
				throw new ArgumentException("Value must be a numeric type.");
		}
	}
}