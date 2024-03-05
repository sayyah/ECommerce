namespace ECommerce.Domain.Exceptions.BlogCategoryExceptions;

    public class RepetitiveNameBlogCategoryException(string blogCategoryName) : KnownException($" نام دسته مقاله تکراری است {blogCategoryName} ")
    {
        public string ErrorMessage { get; private set; } = blogCategoryName;

        public override string GetErrorCode()
        {
            return "BlogCategoryNameISRepetitive";
        }

        public override object GetErrorContextOrDefault()
        {
            return new
            {
                ErrorMessage
            };
        }
    }
