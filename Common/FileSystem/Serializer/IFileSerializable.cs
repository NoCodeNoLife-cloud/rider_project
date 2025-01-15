namespace Common.FileSystem.Serializer;

public interface IFileSerializable
{
	public static abstract void SerializeToFile<T>(T obj, string filePath);

	public static abstract T? DeserializeFromFile<T>(string filePath);
}