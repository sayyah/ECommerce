﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities;

public class Transaction : BaseEntity
{
    [Display(Name = "تاریخ فاکتور")] public DateTime TransactionDate { get; set; }

    [Display(Name = "قابل پرداخت")]
    [Required(ErrorMessage = @"{0} را وارد کنید")]
    public decimal Amount { get; set; }

    [StringLength(20)]
    [Display(Name = "کد رهگیری")]
    public string? RefId { get; set; }

    [Display(Name = "شناسه پرداخت")] public double? PaymentId { get; set; }

    [Display(Name = "کد سند هلو")] public int? SanadCode { get; set; }
    [Display(Name = "کد سند کاربر")] public int? SanadCodeCustomer { get; set; }

    //ForeignKey
    public PurchaseOrder? PurchaseOrder { get; set; }
    public int? PurchaseOrderId { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }

    public int? HolooCompanyId { get; set; } = 1;
    public HolooCompany? HolooCompany { get; set; }
}