using System.Collections;

namespace Common.Check;

[AttributeUsage(AttributeTargets.Parameter)]
public class SizeAttribute(int min, int max) : Attribute, IParameterCheck
{
	private int Min { get; } = min;
	private int Max { get; } = max;

	public void Check(object? value)
	{
		switch (value)
		{
			case null:
				throw new ArgumentException("Value cannot be null.");
			case string str:
			{
				var length = str.Length;
				if (length < Min || length > Max)
				{
					throw new ArgumentException($"String length must be between {Min} and {Max} characters.");
				}

				break;
			}
			case ICollection collection:
			{
				var count = collection.Count;
				if (count < Min || count > Max)
				{
					throw new ArgumentException($"Collection size must be between {Min} and {Max}.");
				}

				break;
			}
			default:
			{
				if (value is Array array)
				{
					var length = array.Length;
					if (length < Min || length > Max)
					{
						throw new ArgumentException($"Array length must be between {Min} and {Max}.");
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