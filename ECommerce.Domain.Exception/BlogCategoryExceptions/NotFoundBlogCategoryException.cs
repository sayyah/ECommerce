namespace ECommerce.Domain.Exceptions.BlogCategoryExceptions;

    public class NotFoundBlogCategoryException(string name) : KnownException($" دسته مقاله یافت نشد {name} ")
    {
        public string ErrorMessage { get; private set; } = name;

        public override string GetErrorCode()
        {
            return "BlogCategoryNotFound";
        }

        public override object GetErrorContextOrDefault()
        {
            return new
            {
                ErrorMessage
            };
        }
}

