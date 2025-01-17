using Common.Check;
using MessagePack;
using MessagePack.Resolvers;

namespace Common.FileSystem.Serializer;

public abstract class BinaryFileSerializer : IFileSerializable
{
	[CheckParameter]
	public static void SerializeToFile<T>([NotNull] T obj, [NotBlank] string filePath)
	{
		var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
		var binaryData = MessagePackSerializer.Serialize(obj, options);
		File.WriteAllBytes(filePath, binaryData);
	}

	[CheckParameter]
	public static T? DeserializeFromFile<T>([FileExists] string filePath)
	{
		var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
		var binaryData = File.ReadAllBytes(filePath);
		return MessagePackSerializer.Deserialize<T>(binaryData, options);
	}
}