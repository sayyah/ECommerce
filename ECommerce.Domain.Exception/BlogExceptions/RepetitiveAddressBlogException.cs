namespace ECommerce.Domain.Exceptions.BlogExceptions
{
    public class RepetitiveAddressBlogException(string blogTitle) : KnownException($"آدرس مقاله تکراری است {blogTitle}")
    {
        public string ErrorMessage { get; private set; } = blogTitle;

        public override string GetErrorCode()
        {
            return "BlogAddressIsRepetitive";
        }

        public override object GetErrorContextOrDefault()
        {
            return new
            {
                ErrorMessage
            };
        }
    }
}
