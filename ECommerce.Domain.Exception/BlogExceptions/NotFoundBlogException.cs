namespace ECommerce.Domain.Exceptions.BlogExceptions;

public class NotFoundBlogException(string title) : KnownException($"مقاله یافت نشد {title}")
{
    public string ErrorMessage { get; private set; } = title;

    public override string GetErrorCode()
    {
        return "BlogNotFound";
    }

    public override object GetErrorContextOrDefault()
    {
        return new
        {
            ErrorMessage
        };
    }
}

