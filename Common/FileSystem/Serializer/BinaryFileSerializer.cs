using MessagePack;
using MessagePack.Resolvers;

namespace Common.FileSystem.Serializer;

public abstract class BinaryFileSerializer : IFileSerializable
{
	public static void SerializeToFile<T>(T obj, string filePath)
	{
		if (obj == null) throw new ArgumentNullException(nameof(obj), "obj cannot be null");
		if (string.IsNullOrEmpty(filePath))
		{
			throw new ArgumentException("file path cannot be empty", nameof(filePath));
		}

		var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
		var binaryData = MessagePackSerializer.Serialize(obj, options);
		File.WriteAllBytes(filePath, binaryData);
	}

	public static T? DeserializeFromFile<T>(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			throw new ArgumentException("file path cannot be empty", nameof(filePath));
		}

		var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
		var binaryData = File.ReadAllBytes(filePath);
		return MessagePackSerializer.Deserialize<T>(binaryData, options);
	}
}