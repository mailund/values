using Shouldly;
using Values;

namespace Blocks.Tests;

public class ContextValueTests
{
    [ Fact ]
    public void ContextAwareBlock_ShouldAccessContextDataThroughLenses()
    {
        // Arrange - Create a ContextRef that will hold the execution context
        var contextRef = new ContextRef();

        // Create ContextValues using the lenses to extract data from context
        var booksContextValue = new ContextValue<IReadOnlyList<string>>(contextRef, ContextLenses.BooksLense);
        var flexContextValue = new ContextValue<int>(contextRef, ContextLenses.FlexLense);

        // Create output value to capture results
        var resultValue = new ClosureValue<string>("");

        // Set up factory and register values
        var factory = new BlockFactory();
        factory.RegisterValue("books", booksContextValue);
        factory.RegisterValue("flex", flexContextValue);
        factory.RegisterValue("result", (IOutValue<string>)resultValue);

        // Create the block
        var block = factory.CreateBlock<ContextAwareBlock>();

        // Test Case 1: Empty context
        var context1 = new TestContext { Books = Array.Empty<string>(), Flex = 0 };
        contextRef.ExecutionContext = context1;

        block.Execute();
        resultValue.Value.ShouldBe("Books: [], Flex: 0");

        // Test Case 2: Context with some books and flex
        var context2 = new TestContext
        {
            Books = new[] { "Book1", "Book2" },
            Flex = 42
        };
        contextRef.ExecutionContext = context2;

        block.Execute();
        resultValue.Value.ShouldBe("Books: [Book1, Book2], Flex: 42");

        // Test Case 3: Context with many books and different flex
        var context3 = new TestContext
        {
            Books = new[] { "Science", "Fiction", "Fantasy", "Mystery" },
            Flex = 100
        };
        contextRef.ExecutionContext = context3;

        block.Execute();
        resultValue.Value.ShouldBe("Books: [Science, Fiction, Fantasy, Mystery], Flex: 100");
    }
}