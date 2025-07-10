using Blocks;

namespace ExecutionPlan;

/// <summary>
///     Represents an execution plan that defines how blocks should be executed and their dependencies
/// </summary>
public class ExecutionPlan
{
    private readonly List<IBlock> _blocks = [];

    public ContextRef ContextRef { get; } = new();

    /// <summary>
    ///     Add a block to the execution plan with its own value namespace
    /// </summary>
    public void AddBlock<T>(Action<BlockFactory> configureValues) where T : IBlock
    {
        var factory = new BlockFactory();
        configureValues(factory);
        var block = factory.CreateBlock<T>();
        _blocks.Add(block);
    }


    /// <summary>
    ///     Execute all blocks in the plan in the order they were added
    /// </summary>
    public void Execute(IContext context)
    {
        ContextRef.ExecutionContext = context;
        foreach (var block in _blocks)
        {
            block.Execute();
        }
    }
}