namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class NotNull : Attribute, IParameterCheck
{
	public void Check(object? value)
	{
		if (value == null)
		{
			throw new ArgumentNullException(nameof(value), "value cannot be null");
		}
	}
}