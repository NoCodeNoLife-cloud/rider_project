using System.Text.Json;

namespace Common.FileSystem.Serializer;

public static class JsonFileSerializer
{
	public static void SerializerToFile<T>(T obj, string filePath, JsonSerializerOptions options)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj), "obj is null");
		}

		var json = JsonSerializer.Serialize(obj, options);
		File.WriteAllText(filePath, json);
	}

	public static T DeserializeFromFile<T>(string filePath)
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