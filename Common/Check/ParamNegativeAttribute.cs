namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class ParamNegativeAttribute : Attribute, IParameterCheck
{
	public void Check(object? value)
	{
		if (value is not IConvertible convertibleValue) return;
		if (convertibleValue.ToInt32(null) > 0)
		{
			throw new ArgumentOutOfRangeException(nameof(value), "value must be negative");
		}
	}
}