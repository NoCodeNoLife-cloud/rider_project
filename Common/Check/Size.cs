using System.Collections;

namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class Size(int minValue, int maxValue) : Attribute, IParameterCheck
{
	private int MinValue { get; } = minValue;
	private int MaxValue { get; } = maxValue;

	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case string str:
			{
				var length = str.Length;
				if (length < MinValue || length > MaxValue)
				{
					throw new ArgumentException($"String length must be between {MinValue} and {MaxValue} characters.");
				}

				break;
			}
			case ICollection collection:
			{
				var count = collection.Count;
				if (count < MinValue || count > MaxValue)
				{
					throw new ArgumentException($"Collection size must be between {MinValue} and {MaxValue}.");
				}

				break;
			}
			default:
			{
				if (value is Array array)
				{
					var length = array.Length;
					if (length < MinValue || length > MaxValue)
					{
						throw new ArgumentException($"Array length must be between {MinValue} and {MaxValue}.");
					}
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