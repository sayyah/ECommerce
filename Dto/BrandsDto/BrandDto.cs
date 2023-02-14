using ECommerce.Dto.Base;

namespace ECommerce.Dto.BrandsDtos;

public class BrandDto : BaseDto
{
    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }

}
