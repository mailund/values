namespace Blocks;

/// <summary>
///     Implementation of block-based functionality
/// </summary>
public abstract class Block : IBlock
{
    public abstract void Execute();
}