using System.Collections;

namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateNotEmptyAttribute : Attribute, IValidateParameter
{
    public void Check(object? value)
    {
        switch (value)
        {
            case null:
                throw new ArgumentException("Value cannot be null.");
            case string str:
            {
                if (string.IsNullOrEmpty(str)) throw new ArgumentException("String cannot be null or empty.");

                break;
            }
            case ICollection collection:
            {
                if (collection.Count == 0) throw new ArgumentException("Collection cannot be null or empty.");

                break;
            }
            default:
            {
                if (value is Array array)
                {
                    if (array.Length == 0) throw new ArgumentException("Array cannot be null or empty.");
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