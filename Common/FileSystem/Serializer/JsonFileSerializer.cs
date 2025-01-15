using System.Text.Json;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.FileSystem.Serializer;

public abstract class JsonFileSerializer : IFileSerializable
{
	public static void SerializeToFile<T>(T obj, string filePath)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj), "obj is null");
		}

		var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
		var json = JsonSerializer.Serialize(obj, options);
		File.WriteAllText(filePath, json);
	}

	public static T? DeserializeFromFile<T>(string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException(nameof(filePath), filePath);
		}

		var json = File.ReadAllText(filePath);
		var obj = JsonSerializer.Deserialize<T>(json);
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(filePath), "file is empty");
		}

		return obj;
	}
}