namespace Common.Validate;

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidateFileExistsAttribute : Attribute, IValidateParameter
{
    public void Check(object? value)
    {
        switch (value)
        {
            case null:
                throw new ArgumentException("Value cannot be null.");
            case string path:
                if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("File path cannot be null or whitespace.");

                if (!File.Exists(path)) throw new ArgumentException($"The file at path '{path}' does not exist.");

                break;
            default:
                throw new ArgumentException("Value must be a string representing a file path.");
        }
    }
}