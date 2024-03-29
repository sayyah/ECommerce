﻿namespace ECommerce.Domain.Entities.HolooEntity;

public class HolooFBail : BaseHolooEntity
{
    public string Fac_Code { get; set; }
    public string Fac_Type { get; set; } = "p";
    public int? Fac_Code_C { get; set; }
    public DateTime? Fac_Date { get; set; }
    public DateTime? Fac_Time { get; set; }
    public string? C_Code { get; set; }
    public double? Sum_Price { get; set; }
    public double? Takhfif { get; set; }
    public string? Fac_Comment { get; set; }
    public int? UserCode { get; set; }
}
