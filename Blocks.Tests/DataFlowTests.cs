using Shouldly;
using Values;

namespace Blocks.Tests;

public class DataFlowTests
{
    [Fact]
    public void ThreeBlocks_ShouldPassDataThroughChain()
    {
        // Arrange - Create the value connectors between blocks using ClosureValue
        var sourceToTransform = new ClosureValue<int>(0);
        var transformToSink = new ClosureValue<string>("");
        var finalResult = new ClosureValue<bool>(false);

        // Create separate factories to avoid name conflicts
        var sourceFactory = new BlockFactory();
        var transformFactory = new BlockFactory();
        var sinkFactory = new BlockFactory();

        // Register values for each block separately
        sourceFactory.RegisterValue("output", sourceToTransform);

        transformFactory.RegisterValue("input", sourceToTransform);
        transformFactory.RegisterValue("output", transformToSink);

        sinkFactory.RegisterValue("input", transformToSink);
        sinkFactory.RegisterValue("result", finalResult);

        // Create blocks using their respective factories
        var sourceBlock = sourceFactory.CreateBlock<SourceBlock>();
        var transformBlock = transformFactory.CreateBlock<TransformBlock>();
        var sinkBlock = sinkFactory.CreateBlock<SinkBlock>();

        // Create test context to capture log messages
        var context = new TestContext();

        // Act - Execute the blocks in sequence
        sourceBlock.Execute(context);
        transformBlock.Execute(context);
        sinkBlock.Execute(context);

        // Assert - Verify the data flowed correctly through all blocks
        finalResult.Value.ShouldBeTrue();

        // Verify intermediate values
        sourceToTransform.Value.ShouldBe(42);
        transformToSink.Value.ShouldBe("Transformed: 84");

        // Verify log messages show the data flow
        context.LogMessages.Count.ShouldBe(3);
        context.LogMessages[0].ShouldBe("SourceBlock: Generating value 42");
        context.LogMessages[1].ShouldBe("TransformBlock: 42 -> Transformed: 84");
        context.LogMessages[2].ShouldBe("SinkBlock: Received 'Transformed: 84', Success: True");
    }
}