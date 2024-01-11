namespace ECommerce.API.DataTransferObject.Tags;
public class TagDto : BaseDto, ITagDto
{
    public string? TagText { get; set; }
}
