using Common.Validate;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Common.FileSystem.Serializer;

public abstract class YamlFileSerializer : IFileSerializable
{
    public static void SerializeToFile<T>([ValidateNotNull] T obj, [ValidateNotBlank] string filePath)
    {
        var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var yaml = serializer.Serialize(obj);
        File.WriteAllText(yaml, filePath);
    }

    public static T? DeserializeFromFile<T>([ValidateFileExists] string filePath)
    {
        var yaml = File.ReadAllText(filePath);
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        return deserializer.Deserialize<T>(yaml);
    }
}