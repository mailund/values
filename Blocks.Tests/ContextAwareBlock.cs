using Values;

namespace Blocks.Tests;

/// <summary>
///     Block that demonstrates accessing context data through ContextValue
/// </summary>
public class ContextAwareBlock(
    [ Value("books") ]  IInValue<IReadOnlyList<string>> books, // it will get this from the context
    [ Value("flex") ]   IInValue<int>                   flex,  // it will get this from the context
    [ Value("result") ] IOutValue<string>               result)
    : Block
{
    public override void Execute()
    {
        var bookList = books.Value;
        var flexValue = flex.Value;


        var summary = $"Books: [{string.Join(", ", bookList)}], Flex: {flexValue}";
        result.SetValue(summary);
    }
}