namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class NotNullAttribute : Attribute, IParameterCheck
{
	public static void Check(object? value)
	{
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value), "value cannot be null");
		}
	}
}