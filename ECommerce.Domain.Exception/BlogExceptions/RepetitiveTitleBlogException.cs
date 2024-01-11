namespace ECommerce.Domain.Exceptions.BlogExceptions
{
    public class RepetitiveTitleBlogException(string blogTitle) : KnownException($"عنوان مقاله تکراری است {blogTitle}")
    {
        public string ErrorMessage { get; private set; } = blogTitle;

        public override string GetErrorCode()
        {
            return "BlogTitleIsRepetitive";
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
