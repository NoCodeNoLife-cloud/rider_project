namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateNotNullAttribute : Attribute, IValidateParameter
{
	public void Check(object? value)
	{
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value), "value cannot be null");
		}
	}
}