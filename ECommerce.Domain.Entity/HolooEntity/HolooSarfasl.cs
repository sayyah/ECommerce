namespace ECommerce.Domain.Entities.HolooEntity;

public class HolooSarfasl : BaseHolooEntity
{
    public string Col_Code { get; set; }
    public string Moien_Code { get; set; }
    public string Tafzili_Code { get; set; }
    public string? Sarfasl_Code { get; set; }
    public string? Sarfasl_Name { get; set; }
    public double? Mandeh { get; set; }
    public short? Group { get; set; }
    public short? Mahiat { get; set; }
    public bool? Can_Delete { get; set; }
    public string? Common { get; set; }
    public bool AutoUse { get; set; }
    public int ID { get; set; }
    public int? Parent { get; set; }
    public int? Type { get; set; }
    public int? SParent { get; set; }
    public int ArzId { get; set; }
    public double Money_Price { get; set; }
    public bool Selected { get; set; }
}
