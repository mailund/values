using Blocks.Tests;
using Shouldly;
using Values;

namespace ExecutionPlan.Tests;

public class ExecutionPlanTests
{
    [ Fact ]
    public void ExecutionPlan_ShouldExecuteBlocksWithSharedValues()
    {
        // Arrange - Create an execution plan
        var plan = new ExecutionPlan();

        // Create shared values that will connect the blocks
        var sourceToTransform = new ClosureValue<int>(0);
        var transformToSink = new ClosureValue<string>("");
        var finalResult = new ClosureValue<bool>(false);

        // Add SourceBlock - generates initial data
        plan.AddBlock<SourceBlock>(factory => { factory.RegisterValue("output", (IOutValue<int>)sourceToTransform); });

        // Add TransformBlock - transforms the data
        plan.AddBlock<TransformBlock>(factory =>
        {
            factory.RegisterValue("input", (IInValue<int>)sourceToTransform);
            factory.RegisterValue("output", (IOutValue<string>)transformToSink);
        });

        // Add SinkBlock - consumes final data
        plan.AddBlock<SinkBlock>(factory =>
        {
            factory.RegisterValue("input", (IInValue<string>)transformToSink);
            factory.RegisterValue("result", (IOutValue<bool>)finalResult);
        });

        // Create execution context
        var context = new TestContext
        {
            Books = ["C# Programming", "Design Patterns"],
            Flex = 75
        };

        // Act - Execute the plan
        plan.Execute(context);

        // Assert - Verify the data flowed correctly through all blocks
        sourceToTransform.Value.ShouldBe(42);
        transformToSink.Value.ShouldBe("Transformed: 84");
        finalResult.Value.ShouldBeTrue();
    }

    [ Fact ]
    public void ExecutionPlan_ShouldWorkWithContextValues()
    {
        // Arrange - Create an execution plan
        var plan = new ExecutionPlan();

        // Create context values using the plan's ContextRef
        var booksValue = plan.ContextRef.BooksValue;
        var flexValue = plan.ContextRef.FlexValue;
        var contextResult = new ClosureValue<string>("");

        // Add ContextAwareBlock - uses context data
        plan.AddBlock<ContextAwareBlock>(factory =>
        {
            factory.RegisterValue("books", booksValue);
            factory.RegisterValue("flex", flexValue);
            factory.RegisterValue("result", (IOutValue<string>)contextResult);
        });

        // Create execution context with data
        var context = new TestContext
        {
            Books = ["Clean Code", "Refactoring", "Design Patterns"],
            Flex = 100
        };

        // Act - Execute the plan
        plan.Execute(context);

        // Assert - Verify context values were accessed correctly
        contextResult.Value.ShouldBe("Books: [Clean Code, Refactoring, Design Patterns], Flex: 100");
    }
}