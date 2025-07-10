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

    /// <summary>
    ///     Filters an enumerable value using the provided predicate
    /// </summary>
    /// <param name="input">The input enumerable value</param>
    /// <param name="predicate">The filter predicate</param>
    /// <typeparam name="T">The element type</typeparam>
    /// <returns>A new IInValue containing the filtered enumerable</returns>
    public static IInValue<IEnumerable<T>> Filter<T>(this IInValue<IEnumerable<T>> input, Func<T, bool> predicate)
    {
        return new ClosureValue<IEnumerable<T>>(() => input.Value.Where(predicate));
    }
}

public class ClosureValue<T>(Func<T> closure) : IInValue<T>, IOutValue<T>
{
    private Func<T> _closure = closure;

    // Constructor that takes a default value and wraps it in a closure
    public ClosureValue(T defaultValue) : this(() => defaultValue) { }

    public T Value => _closure();

    public void SetValue(Func<T> valueFunc)
    {
        _closure = valueFunc;
    }
}