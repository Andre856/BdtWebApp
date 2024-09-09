using Bdt.Api.Domain.Enums;

namespace Bdt.Api.Infrastructure.Exceptions.Database;
public class DbOperationFailedException : Exception
{
    public Exception? Inner { get; }
    public DbOperationEnum Operation { get; private set; }

    public DbOperationFailedException(DbOperationEnum operation,  string message, Exception? innerException = null) : base(message, innerException)
    {
        Inner = InnerException;
        Operation = operation;
    }
}
