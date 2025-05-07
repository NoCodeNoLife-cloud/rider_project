using System.Collections;

namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateSizeAttribute(int minValue, int maxValue) : Attribute, IValidateParameter
{
    public void Check(object? value)
    {
        switch (value)
        {
            case null:
                throw new ArgumentException("Value cannot be null.");
            case string str:
            {
                var length = str.Length;
                if (length < minValue || length > maxValue) throw new ArgumentException($"String length must be between {minValue} and {maxValue} characters.");

                break;
            }
            case ICollection collection:
            {
                var count = collection.Count;
                if (count < minValue || count > maxValue) throw new ArgumentException($"Collection size must be between {minValue} and {maxValue}.");

                break;
            }
            default:
            {
                if (value is Array array)
                {
                    var length = array.Length;
                    if (length < minValue || length > maxValue) throw new ArgumentException($"Array length must be between {minValue} and {maxValue}.");
                }
                else
                {
                    throw new ArgumentException("Value must be a string, array, or collection.");
                }

                break;
            }
        }
    }
}