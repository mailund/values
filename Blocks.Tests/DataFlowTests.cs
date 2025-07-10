using Shouldly;
using Values;

namespace Blocks.Tests;

public class DataFlowTests
{
    [ Fact ]
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

        // Register values for each block with explicit interface types
        sourceFactory.RegisterValue("output", (IOutValue<int>)sourceToTransform);

        transformFactory.RegisterValue("input", (IInValue<int>)sourceToTransform);
        transformFactory.RegisterValue("output", (IOutValue<string>)transformToSink);

        sinkFactory.RegisterValue("input", (IInValue<string>)transformToSink);
        sinkFactory.RegisterValue("result", (IOutValue<bool>)finalResult);

        // Create blocks using their respective factories
        var sourceBlock = sourceFactory.CreateBlock<SourceBlock>();
        var transformBlock = transformFactory.CreateBlock<TransformBlock>();
        var sinkBlock = sinkFactory.CreateBlock<SinkBlock>();

        // Act - Execute the blocks in sequence
        sourceBlock.Execute();
        transformBlock.Execute();
        sinkBlock.Execute();

        // Assert - Verify the data flowed correctly through all blocks
        finalResult.Value.ShouldBeTrue();

        // Verify intermediate values
        sourceToTransform.Value.ShouldBe(42);
        transformToSink.Value.ShouldBe("Transformed: 84");

        // Note: We can no longer verify log messages since blocks don't have access to context logging
    }
}