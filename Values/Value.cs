namespace Values;

/// <summary>
///     A read-only value.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IInValue<out T>
{
    T Value { get; }
}

/// <summary>
///     A write-only value.
/// </summary>
public interface IOutValue<in T>
{
    void SetValue(Func<T> valueFunc);
}

public interface IInOutValue<T> : IInValue<T>, IOutValue<T> { }

public static class ValueExtensions
{
    /// <summary>
    ///     Sets the value of an IInOutValue by wrapping the value in a closure.
    ///     This is an extension method because default implementations in interfaces
    ///     are not propagated to derived classes.
    /// </summary>
    /// <param name="outValue"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void SetValue<T>(this IOutValue<T> outValue, T value)
    {
        outValue.SetValue(() => value);
    }
}

public class ClosureValue<T>(T defaultValue, Func<T>? closure = null) : IInOutValue<T>
{
    private Func<T> _closure = closure ?? (() => defaultValue);

    public T Value => _closure();

    public void SetValue(Func<T> valueFunc)
    {
        _closure = valueFunc;
    }
}