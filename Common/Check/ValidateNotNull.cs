namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateNotNull : Attribute, IValidateParameter
{
	public void Check(object? value)
	{
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value), "value cannot be null");
		}
	}
}