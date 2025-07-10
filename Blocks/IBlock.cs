namespace Blocks;

/// <summary>
///     Interface for block-based functionality
/// </summary>
public interface IBlock
{
    void Execute(IContext context);
}