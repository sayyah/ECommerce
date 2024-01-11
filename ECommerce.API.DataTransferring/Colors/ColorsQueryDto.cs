using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferring.Colors;

public class ColorsQueryDto
{
    public int Id { get; set; }
    public PaginationParameters? PaginationParameters { get; set; }
}
