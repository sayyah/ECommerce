using ECommerce.Domain.Entities.Helper;

namespace ECommerce.Application.DataTransferObjects.Color;

public class ColorsQueryDto
{
    public int Id { get; set; }
    public PaginationParameters? PaginationParameters { get; set; }
}
