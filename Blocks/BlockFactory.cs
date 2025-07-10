using System.Reflection;
using Values;

namespace Blocks;

/// <summary>
///     Generic factory for creating blocks with injected values as constructor arguments using reflection
/// </summary>
public class BlockFactory
{
    private readonly Dictionary<string, object> _values = new();

    /// <summary>
    ///     Register a value with a specific name
    /// </summary>
    public void RegisterValue<T>(string name, IInValue<T> value)
    {
        _values[name] = value;
    }

    /// <summary>
    ///     Register an output value with a specific name
    /// </summary>
    public void RegisterValue<T>(string name, IOutValue<T> value)
    {
        _values[name] = value;
    }

    /// <summary>
    ///     Register an input/output value with a specific name
    /// </summary>
    public void RegisterValue<T>(string name, IInOutValue<T> value)
    {
        _values[name] = value;
    }

    /// <summary>
    ///     Create a block of type T using reflection to inject constructor parameters based on ValueAttribute
    /// </summary>
    public T CreateBlock<T>() where T : IBlock
    {
        var blockType = typeof(T);
        var constructors = blockType.GetConstructors();

        if (constructors.Length != 1)
        {
            throw new InvalidOperationException($"Block type {blockType.Name} must have exactly one constructor");
        }

        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var valueAttribute = parameter.GetCustomAttribute<ValueAttribute>();

            if (valueAttribute == null)
            {
                throw new InvalidOperationException(
                    $"Constructor parameter '{parameter.Name}' in {blockType.Name} must have a ValueAttribute");
            }

            var valueName = valueAttribute.Name ?? parameter.Name;

            if (_values.TryGetValue(valueName!, out var value))
            {
                if (parameter.ParameterType.IsAssignableFrom(value.GetType()))
                {
                    args[i] = value;
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Value '{valueName}' is not compatible with parameter '{parameter.Name}' of type {parameter.ParameterType} in {blockType.Name}");
                }
            }
            else if (valueAttribute.Required)
            {
                throw new InvalidOperationException(
                    $"Required value '{valueName}' not found for parameter '{parameter.Name}' in {blockType.Name}");
            }
            else
            {
                args[i] = null!; // Optional parameter
            }
        }

        return (T)Activator.CreateInstance(blockType, args)!;
    }
}