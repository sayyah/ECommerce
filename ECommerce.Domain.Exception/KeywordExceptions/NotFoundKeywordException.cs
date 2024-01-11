namespace ECommerce.Domain.Exceptions.KeywordExceptions;

public class NotFoundKeywordException(int id) : KnownException($"کلمه کلیدی یافت نشد {id}")
{
    public int ErrorMessage { get; private set; } = id;

    public override string GetErrorCode()
    {
        return "KeywordNotFound";
    }

    public override object GetErrorContextOrDefault()
    {
        return new
        {
            ErrorMessage
        };
    }
}
