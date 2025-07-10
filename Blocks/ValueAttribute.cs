namespace Blocks;

/// <summary>
///     Attribute to mark constructor parameters that should be injected with Values
/// </summary>
[ AttributeUsage(AttributeTargets.Parameter) ]
public class ValueAttribute : Attribute
{
    /// <summary>
    ///     Creates a new ValueAttribute
    /// </summary>
    public ValueAttribute() { }

    /// <summary>
    ///     Creates a new ValueAttribute with a specific name
    /// </summary>
    /// <param name="name">The name for value binding</param>
    public ValueAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     Optional name for the value binding. If not provided, the parameter name will be used.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Whether this value is required for the block to execute
    /// </summary>
    public bool Required { get; set; } = true;
}