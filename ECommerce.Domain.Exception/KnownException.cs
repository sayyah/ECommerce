namespace ECommerce.Domain.Exceptions;
public abstract class KnownException : Exception
{
    protected KnownException(string message)
        : base(message)
    {
    }

    protected KnownException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public abstract string GetErrorCode();

    public virtual object? GetErrorContextOrDefault()
    {
        return default;
    }
}
