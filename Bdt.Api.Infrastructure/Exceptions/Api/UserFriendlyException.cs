namespace Bdt.Api.Infrastructure.Exceptions.Api;
public class UserFriendlyException : Exception
{
    public Exception? Inner { get; }

    public UserFriendlyException(string message, Exception? innerException = null) : base(message, innerException)
    {
        Inner = innerException;
    }
}
