namespace ECommerce.API.DataTransferObject.Colors;

public class ColorUpdateDto : ColorDto
{
    public int Id { get; set; }
    public int? EditorUserId { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
