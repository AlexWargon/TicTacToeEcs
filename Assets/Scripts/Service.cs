using System.Runtime.CompilerServices;

public static class Service<T> where T : class {
    private static T _instance;

    public static void Set(T value) {
        _instance = value;
        if (value is IInitService init)
            init.Init();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Get() {
        return _instance;
    }
}

public interface IInitService {
    void Init();
}