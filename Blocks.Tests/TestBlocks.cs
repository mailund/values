using Values;

namespace Blocks.Tests;

/// <summary>
///     First block in the chain - generates initial data
/// </summary>
public class SourceBlock : Block
{
    private readonly IOutValue<int> _output;

    public SourceBlock([ Value ] IOutValue<int> output)
    {
        _output = output;
    }

    public override void Execute(IContext context)
    {
        var value = 42;
        context.Log($"SourceBlock: Generating value {value}");
        _output.SetValue(value);
    }
}

/// <summary>
///     Middle block in the chain - transforms data
/// </summary>
public class TransformBlock : Block
{
    private readonly IInValue<int>     _input;
    private readonly IOutValue<string> _output;

    public TransformBlock(
        [ Value ] IInValue<int>     input,
        [ Value ] IOutValue<string> output)
    {
        _input = input;
        _output = output;
    }

    public override void Execute(IContext context)
    {
        var inputValue = _input.Value;
        var transformedValue = $"Transformed: {inputValue * 2}";
        context.Log($"TransformBlock: {inputValue} -> {transformedValue}");
        _output.SetValue(transformedValue);
    }
}

/// <summary>
///     Final block in the chain - consumes data
/// </summary>
public class SinkBlock : Block
{
    private readonly IInValue<string> _input;
    private readonly IOutValue<bool>  _result;

    public SinkBlock(
        [ Value ] IInValue<string> input,
        [ Value ] IOutValue<bool>  result)
    {
        _input = input;
        _result = result;
    }

    public override void Execute(IContext context)
    {
        var inputValue = _input.Value;
        var success = !string.IsNullOrEmpty(inputValue);
        context.Log($"SinkBlock: Received '{inputValue}', Success: {success}");
        _result.SetValue(success);
    }
}