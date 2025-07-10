using Values;

namespace Blocks.Tests;

/// <summary>
///     First block in the chain - generates initial data
/// </summary>
public class SourceBlock([ Value ] IOutValue<int> output) : Block
{
    public override void Execute()
    {
        var value = 42;
        output.SetValue(value);
    }
}

/// <summary>
///     Middle block in the chain - transforms data
/// </summary>
public class TransformBlock([ Value ] IInValue<int> input, [ Value ] IOutValue<string> output)
    : Block
{
    public override void Execute()
    {
        var inputValue = input.Value;
        var transformedValue = $"Transformed: {inputValue * 2}";
        output.SetValue(transformedValue);
    }
}

/// <summary>
///     Final block in the chain - consumes data
/// </summary>
public class SinkBlock([ Value ] IInValue<string> input, [ Value ] IOutValue<bool> result)
    : Block
{
    public override void Execute()
    {
        var inputValue = input.Value;
        var success = !string.IsNullOrEmpty(inputValue);
        result.SetValue(success);
    }
}