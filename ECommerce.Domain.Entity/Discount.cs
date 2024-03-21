using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class Discount : BaseEntity
{
    [Display(Name = "درصد")] public double? Percent { get; set; }

    [Display(Name = "مبلغ")] public int? Amount { get; set; }

    [Display(Name = "نام")]
    [Required(ErrorMessage = @"{0} را وارد کنید")]
    public string Name { get; set; }

    [Display(Name = "تاریخ شروع")] public DateTime? StartDate { get; set; }

    [Display(Name = "تاریخ پایان")] public DateTime? EndDate { get; set; }

    [Display(Name = "سقف مبلغ برای درصد")] public int? MaxAmount { get; set; }

    [Display(Name = "حداقل سفارش")] public int? MinOrder { get; set; }

    [Display(Name = "حداکثر سفارش")] public int? MaxOrder { get; set; }

    [Display(Name = "فعال")] public bool IsActive { get; set; } = false;

    [StringLength(40)]
    [Display(Name = "کد تخفیف")]
    public string? Code { get; set; }

    [Display(Name = "تعداد کوپن")] public int? CouponQty { get; set; }
    [Display(Name = "نوع تخفیف")] public DiscountType? DiscountType { get; set; }

    //ForeignKey

    public ICollection<Product>? Products { get; set; }
    public ICollection<Price>? Prices { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
    public ICollection<PurchaseOrderDetail>? PurchaseOrderDetails { get; set; }
}

public enum DiscountType
{
    Coupon = 0,
    Category = 1,
    Price = 2,
}