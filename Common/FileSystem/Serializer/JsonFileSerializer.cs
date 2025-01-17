using System.Text.Json;
using Common.Log.Serilog;
using Common.Validate;
using Serilog.Events;

namespace Common.FileSystem.Serializer;

public abstract class JsonFileSerializer : IFileSerializable
{
	[ValidateParameter]
	public static void SerializeToFile<T>([ValidateNotNull] T obj, [ValidateNotBlank]string filePath)
	{
		var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
		var json = JsonSerializer.Serialize(obj, options);
		File.WriteAllText(filePath, json);
	}

	[ValidateParameter]
	public static T? DeserializeFromFile<T>([ValidateFileExists] string filePath)
	{
		var json = File.ReadAllText(filePath);
		var obj = JsonSerializer.Deserialize<T>(json);
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(filePath), "file is empty");
		}

		return obj;
	}
}