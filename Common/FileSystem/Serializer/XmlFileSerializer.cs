using System.Xml;
using System.Xml.Serialization;

namespace Common.FileSystem.Serializer;

public abstract class XmlFileSerializer : IFileSerializable
{
	public static void SerializeToFile<T>(T obj, string filePath)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj), "The object to serialize cannot be null.");
		}

		var serializer = new XmlSerializer(typeof(T));
		var settings = new XmlWriterSettings
		{
			Indent = true,
			NewLineOnAttributes = false,
			OmitXmlDeclaration = false
		};

		using var writer = XmlWriter.Create(filePath, settings);
		serializer.Serialize(writer, obj);
	}

	public static T? DeserializeFromFile<T>(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
		}

		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("The specified XML file does not exist.", filePath);
		}

		var serializer = new XmlSerializer(typeof(T));
		using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		return (T?)serializer.Deserialize(stream);
	}
}