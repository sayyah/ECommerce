namespace ECommerce.API.DataTransferObject.Tags;
public class TagDto : BaseDto, ITagDto
{
    public string? TagText { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime CreatorUserId { get; set; } = DateTime.Now;
}
