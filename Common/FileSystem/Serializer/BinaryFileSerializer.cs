using Common.Validate;
using MessagePack;
using MessagePack.Resolvers;

namespace Common.FileSystem.Serializer;

public abstract class BinaryFileSerializer : IFileSerializable
{
    [ValidateParameter]
    public static void SerializeToFile<T>([ValidateNotNull] T obj, [ValidateNotBlank] string filePath)
    {
        var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
        var binaryData = MessagePackSerializer.Serialize(obj, options);
        File.WriteAllBytes(filePath, binaryData);
    }

    [ValidateParameter]
    public static T? DeserializeFromFile<T>([ValidateFileExists] string filePath)
    {
        var options = MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);
        var binaryData = File.ReadAllBytes(filePath);
        return MessagePackSerializer.Deserialize<T>(binaryData, options);
    }
}