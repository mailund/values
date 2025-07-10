namespace Blocks;

public interface IContext
{
    IReadOnlyList<string> Books { get; }
    int                   Flex  { get; }
    void                  Log(string message);
}