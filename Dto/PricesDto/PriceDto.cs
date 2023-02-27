using ECommerce.Dto.Base;

namespace Ecommerce.Dto.PricesDto;

public class PriceDto : BaseDto
{
    public enum HolooSellNumber
    {
        خالی = 0,
        Sel_Price = 1,
        Sel_Price2 = 2,
        Sel_Price3 = 3,
        Sel_Price4 = 4,
        Sel_Price5 = 5,
        Sel_Price6 = 6,
        Sel_Price7 = 7,
        Sel_Price8 = 8,
        Sel_Price9 = 9,
        Sel_Price10 = 10
    }

    public decimal Amount { get; set; }

    public int MinQuantity { get; set; }  

    public int MaxQuantity { get; set; }
     
    public HolooSellNumber? SellNumber { get; set; }

    public string? ArticleCode { get; set; }

    public string? ArticleCodeCustomer { get; set; }

    public double Exist { get; set; }

    //public Grade Grade { get; set; }

    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }

    public int? UnitId { get; set; }
    public UnitDto? Unit { get; set; }

    public int? SizeId { get; set; }
    public SizeDto? Size { get; set; }

    public int? ColorId { get; set; }
    public ColorDto? Color { get; set; }

    public int? CurrencyId { get; set; }
    public CurrencyDto? Currency { get; set; }

}
