namespace Common.Entity;

public static class SingletonObject<T> where T : class
{
    private static readonly Lazy<T?> Instance = new(() => _initializer?.Invoke());

    private static Func<T>? _initializer;

    public static T? instance => Instance.Value;

    public static void Initialize(Func<T> initializer)
    {
        if (_initializer != null) throw new InvalidOperationException("Singleton instance has already been initialized.");

        _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
    }
}