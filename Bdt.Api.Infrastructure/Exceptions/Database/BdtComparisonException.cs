namespace Bdt.Api.Infrastructure.Exceptions.Database;
public class BdtComparisonException : Exception
{
    public Exception? Inner { get; }
    public BdtComparisonException(string message, Exception? inner = null) : base(message, inner)
    {
        Inner = inner;
    }
}
