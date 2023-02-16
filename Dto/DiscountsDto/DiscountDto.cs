﻿using ECommerce.Dto.Base;

namespace ECommerce.Dto.DiscountsDto;

public class DiscountDto : BaseDto
{
    public double? Percent { get; set; }

    public int? Amount { get; set; }

    public string? Name { get; set; }

}
