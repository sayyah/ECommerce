using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.Colors;

public class ColorsQueryDto
{
    public int Id { get; set; }
    public PaginationParameters? PaginationParameters { get; set; }
}
