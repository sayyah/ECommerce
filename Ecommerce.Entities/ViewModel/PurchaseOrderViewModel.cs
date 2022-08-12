﻿using Entities.Helper;

namespace Entities.ViewModel;

public class PurchaseOrderViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Url { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? ImagePath { get; set; }
    public string? Alt { get; set; }
    public int PriceId { get; set; }
    public Price Price { get; set; }
    public decimal PriceAmount { get; set; }
    public decimal SumPrice { get; set; }
    public ushort Quantity { get; set; }
    public double Exist { get; set; }
    public string ColorName { get; set; }
    public bool IsColleague { get; set; }

    //ForeignKey
    public int UserId { get; set; }
}

public class PurchaseFiltreOrderViewModel
{
    public PaginationParameters PaginationParameters { get; set; }
    public int UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsPaied { get; set; }
    public PurchaseSort PurchaseSort { get; set; }
}

public class PurchaseListViewModel
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsPaied { get; set; }
    public string Description { get; set; }
}

public class PurchaseOptinalListViewModel
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int StatusId { get; set; }
    public decimal MinimumAmount { get; set; }
    public decimal MaximumAmount { get; set; }
    public int IsPaid { get; set; }
    public Shipping? Shipping { get; set; }
    public int UserId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}