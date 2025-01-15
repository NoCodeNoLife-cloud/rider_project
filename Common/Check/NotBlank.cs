namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class NotBlank : Attribute, IParameterCheck
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