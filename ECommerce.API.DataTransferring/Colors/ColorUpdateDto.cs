namespace ECommerce.API.DataTransferring.Colors;

public class ColorUpdateDto : ColorDto
{
    public int Id { get; set; }
    public int? EditorUserId { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
