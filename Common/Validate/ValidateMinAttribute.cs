namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateMinAttribute(double minValue) : Attribute, IValidateParameter
{
    private double MinValue { get; } = minValue;

    public void Check(object? value)
    {
        switch (value)
        {
            case null:
                throw new ArgumentException("Value cannot be null.");
            case IComparable comparable:
            {
                if (comparable.CompareTo(MinValue) < 0) throw new ArgumentException($"Value must be greater than or equal to {MinValue}.");

                break;
            }
            default:
                throw new ArgumentException("Value must be a numeric type.");
        }
    }
}