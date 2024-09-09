namespace Bdt.Api.Infrastructure.Exceptions.Database;
public class BdtNotFoundException : Exception
{
    public Exception? Inner { get; }
    public BdtNotFoundException(string message, Exception? inner = null) : base(message, inner)
    {
        Inner = inner;
    }
}
