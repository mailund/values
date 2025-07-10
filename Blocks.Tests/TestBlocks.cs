using Values;

namespace Blocks.Tests;

/// <summary>
///     First block in the chain - generates initial data
/// </summary>
public class SourceBlock([ Value ] IOutValue<int> output) : Block
{
    public override void Execute(IContext context)
    {
        var value = 42;
        context.Log($"SourceBlock: Generating value {value}");
        output.SetValue(value);
    }
}

/// <summary>
///     Middle block in the chain - transforms data
/// </summary>
public class TransformBlock(
    [ Value ] IInValue<int>     input,
    [ Value ] IOutValue<string> output)
    : Block
{
    public override void Execute(IContext context)
    {
        var inputValue = input.Value;
        var transformedValue = $"Transformed: {inputValue * 2}";
        context.Log($"TransformBlock: {inputValue} -> {transformedValue}");
        output.SetValue(transformedValue);
    }
}

/// <summary>
///     Final block in the chain - consumes data
/// </summary>
public class SinkBlock(
    [ Value ] IInValue<string> input,
    [ Value ] IOutValue<bool>  result)
    : Block
{
    public override void Execute(IContext context)
    {
        var inputValue = input.Value;
        var success = !string.IsNullOrEmpty(inputValue);
        context.Log($"SinkBlock: Received '{inputValue}', Success: {success}");
        result.SetValue(success);
    }
}