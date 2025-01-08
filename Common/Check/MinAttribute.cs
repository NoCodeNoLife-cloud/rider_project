namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class MinAttribute(double min) : Attribute, IParameterCheck
{
	private double Min { get; } = min;

	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case IComparable comparable:
			{
				if (comparable.CompareTo(Min) < 0)
				{
					throw new ArgumentException($"Value must be greater than or equal to {Min}.");
				}

				break;
			}
			default:
				throw new ArgumentException("Value must be a numeric type.");
		}
	}
}