namespace ECommerce.API.DataTransferObject.Colors;

public class ColorCreateDto : ColorDto
{
    public int? CreatorUserId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
