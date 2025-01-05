namespace Common.Entity;

public abstract class SingletonWrapper<T> where T : class, new()
{
	private static readonly Lazy<T> Instance = new Lazy<T>(() => new T());

	public static T GetInstance() => Instance.Value;
}