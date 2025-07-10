using Values;

namespace Blocks;

/// <summary>
///     Example block that demonstrates value injection using constructor parameters with
///     InValueAttribute/OutValueAttribute
/// </summary>
public class ExampleBlock(
    [ Value ] IInValue<int>     inputValue,
    [ Value ] IOutValue<double> result,
    [ Value ] IOutValue<string> outputValue)
    : Block
{
    public override void Execute()
    {
        var input = inputValue.Value;

        // Set output value
        outputValue.SetValue($"Processed: {input}");

        // Update result if available
        result.SetValue(input * 2.5);
    }
}