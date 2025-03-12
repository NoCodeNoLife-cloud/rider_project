namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidatePositiveOrZeroAttribute : Attribute, IValidateParameter
{
	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case IComparable comparable:
			{
				if (comparable.CompareTo(0) < 0)
				{
					throw new ArgumentException("Value must be a positive or zero number.");
				}

				break;
			}
			default:
				throw new ArgumentException("Value must be a numeric type.");
		}
	}
}