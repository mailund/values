using Values;

namespace Blocks;

public class ContextRef
{
    public IContext ExecutionContext { get; set; }

    public IInValue<IReadOnlyList<string>> BooksValue =>
        new ContextValue<IReadOnlyList<string>>(this, ContextLenses.BooksLense);

    public IInValue<int> FlexValue => new ContextValue<int>(this, ContextLenses.FlexLense);
}

public class ContextValue<T>(ContextRef contextRef, Func<IContext, T> lense)
    : IInValue<T>
{
    public T Value => lense(contextRef.ExecutionContext);
}

public static class ContextLenses
{
    public static Func<IContext, IReadOnlyList<string>> BooksLense => context => context.Books;
    public static Func<IContext, int>                   FlexLense  => context => context.Flex;
}