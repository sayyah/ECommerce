﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.HolooEntity;

public class HolooMGroup : BaseHolooEntity
{
    [Key] public string M_groupcode { get; set; }

    public string M_groupname { get; set; }
}
