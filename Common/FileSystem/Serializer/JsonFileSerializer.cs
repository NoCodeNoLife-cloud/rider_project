using System.Text.Json;
using Common.Check;
using Common.Log.Serilog;
using Serilog.Events;

namespace Common.FileSystem.Serializer;

public abstract class JsonFileSerializer : IFileSerializable
{
	[CheckParameter]
	public static void SerializeToFile<T>([NotNull] T obj, [NotBlank]string filePath)
	{
		var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
		var json = JsonSerializer.Serialize(obj, options);
		File.WriteAllText(filePath, json);
	}

	[CheckParameter]
	public static T? DeserializeFromFile<T>([FileExists] string filePath)
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