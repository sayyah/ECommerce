namespace ECommerce.Domain.Exceptions.TagException;

public class NotFoundTagException(int id) : KnownException($"تگ یافت نشد {id}")
{
    public int ErrorMessage { get; private set; } = id;

    public override string GetErrorCode()
    {
        return "TagNotFound";
    }

    public override object GetErrorContextOrDefault()
    {
        return new
        {
            ErrorMessage
        };
    }
}
