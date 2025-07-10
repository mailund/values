using Values;

namespace Blocks;

public class ContextRef
{
    public IContext ExecutionContext { get; set; }
}

public class ContextValue<T>(ContextRef contextRef, Func<IContext, T> lense)
    : IInValue<T>
{
    public T Value => lense(contextRef.ExecutionContext);
}

public class ContextLenses
{
    public static Func<IContext, IReadOnlyList<string>> BooksLense => context => context.Books;
    public static Func<IContext, int>                   FlexLense  => context => context.Flex;
}