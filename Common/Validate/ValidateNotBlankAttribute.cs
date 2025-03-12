namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateNotBlankAttribute : Attribute, IValidateParameter
{
	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case string str when string.IsNullOrWhiteSpace(str):
				throw new ArgumentException("String cannot be empty or contain only white-space characters.");
		}
	}
}