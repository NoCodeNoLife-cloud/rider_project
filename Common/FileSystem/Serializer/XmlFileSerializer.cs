using System.Xml;
using System.Xml.Serialization;
using Common.Log.Serilog;
using Common.Validate;
using Serilog.Events;

namespace Common.FileSystem.Serializer;

public abstract class XmlFileSerializer : IFileSerializable
{
	[ValidateParameter]
	public static void SerializeToFile<T>([ValidateNotNull] T obj, [ValidateNotBlank] string filePath)
	{
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

	[ValidateParameter]
	public static T? DeserializeFromFile<T>([ValidateFileExists] string filePath)
	{
		var serializer = new XmlSerializer(typeof(T));
		using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		return (T?)serializer.Deserialize(stream);
	}
}