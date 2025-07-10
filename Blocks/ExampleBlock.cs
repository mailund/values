using Values;

namespace Blocks;

/// <summary>
///     Example block that demonstrates value injection using constructor parameters with ValueAttribute
/// </summary>
public class ExampleBlock : Block
{
    private readonly IInValue<int>       _inputValue;
    private readonly IOutValue<string>   _outputValue;
    private readonly IInOutValue<double> _result;

    public ExampleBlock(
        [ Value("input") ]  IInValue<int>       inputValue,
        [ Value ]           IInOutValue<double> result,
        [ Value("output") ] IOutValue<string>   outputValue)
    {
        _inputValue = inputValue;
        _result = result;
        _outputValue = outputValue;
    }

    public override void Execute(IContext context)
    {
        var input = _inputValue.Value;
        context.Log($"Processing input: {input}");

        // Set output value
        _outputValue.SetValue($"Processed: {input}");

        // Update result if available
        _result.SetValue(input * 2.5);
    }
}