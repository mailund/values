namespace Blocks.Tests;

/// <summary>
///     Test context implementation that captures log messages
/// </summary>
public class TestContext : IContext
{
    public List<string> LogMessages { get; } = new();

    public void Log(string message)
    {
        LogMessages.Add(message);
    }
}