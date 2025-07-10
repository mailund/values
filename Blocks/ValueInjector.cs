using System.Reflection;
using Values;

namespace Blocks;

/// <summary>
///     Service for injecting values into blocks based on ValueAttribute
/// </summary>
public class ValueInjector
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
    ///     Inject values into a block based on ValueAttribute decorations
    /// </summary>
    public void InjectValues(IBlock block)
    {
        var blockType = block.GetType();
        var fields = blockType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var properties = blockType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // Inject into fields
        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<ValueAttribute>();
            if (attribute != null)
            {
                InjectIntoField(block, field, attribute);
            }
        }

        // Inject into properties
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<ValueAttribute>();
            if (attribute != null && property.CanWrite)
            {
                InjectIntoProperty(block, property, attribute);
            }
        }
    }

    private void InjectIntoField(IBlock block, FieldInfo field, ValueAttribute attribute)
    {
        var valueName = attribute.Name ?? field.Name;

        if (_values.TryGetValue(valueName, out var value))
        {
            if (IsCompatibleType(field.FieldType, value.GetType()))
            {
                field.SetValue(block, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Value '{valueName}' is not compatible with field '{field.Name}' of type {field.FieldType}");
            }
        }
        else
        {
            throw new InvalidOperationException(
                $"Required value '{valueName}' not found for field '{field.Name}'");
        }
    }

    private void InjectIntoProperty(IBlock block, PropertyInfo property, ValueAttribute attribute)
    {
        var valueName = attribute.Name ?? property.Name;

        if (_values.TryGetValue(valueName, out var value))
        {
            if (IsCompatibleType(property.PropertyType, value.GetType()))
            {
                property.SetValue(block, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Value '{valueName}' is not compatible with property '{property.Name}' of type {property.PropertyType}");
            }
        }
        else
        {
            throw new InvalidOperationException(
                $"Required value '{valueName}' not found for property '{property.Name}'");
        }
    }

    private static bool IsCompatibleType(Type targetType, Type valueType)
    {
        return targetType.IsAssignableFrom(valueType);
    }
}