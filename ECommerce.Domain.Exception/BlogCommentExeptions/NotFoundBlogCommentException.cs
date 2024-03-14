namespace ECommerce.Domain.Exceptions.BlogCommentExeptions;

public class NotFoundBlogCommentException(int id) : KnownException($"کامنت مقاله موردنظر یافت نشد {id}")
{
    public string ErrorMessage { get; } = id.ToString();

    public override string GetErrorCode()
    {
        return "BlogCommentNotFound";
    }

    public override object GetErrorContextOrDefault()
    {
        return new
        {
            ErrorMessage
        };
    }
}

