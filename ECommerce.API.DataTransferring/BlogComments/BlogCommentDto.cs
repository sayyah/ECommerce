namespace ECommerce.API.DataTransferObject.BlogComments;
public class CommentDto : BaseDto, IBlogCommentDto
{
    public string Text { get; set; }

    public DateTime DateTime { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }
}
