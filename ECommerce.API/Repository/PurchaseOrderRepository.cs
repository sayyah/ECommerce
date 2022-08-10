﻿using API.DataContext;
using API.Interface;
using API.Utilities;
using Entities;
using Entities.Helper;
using Entities.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class PurchaseOrderRepository : AsyncRepository<PurchaseOrder>, IPurchaseOrderRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public PurchaseOrderRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<PurchaseListViewModel>> Search(PurchaseFiltreOrderViewModel purchaseFiltreOrderViewModel,
        CancellationToken cancellationToken)
    {
        var query = _context.PurchaseOrders.Where(x => x.User.UserName.Contains(purchaseFiltreOrderViewModel.PaginationParameters.Search))
                .AsNoTracking();

        if (purchaseFiltreOrderViewModel.IsPaied != null) query = query.Where(x => x.IsPaid == purchaseFiltreOrderViewModel.IsPaied);
        if (purchaseFiltreOrderViewModel.UserId > 0 ) query = query.Where(x => x.UserId == purchaseFiltreOrderViewModel.UserId);

        var sortedQuery = query.OrderByDescending(x => x.Id).ToList();

        switch (purchaseFiltreOrderViewModel.PurchaseSort)
        {
            case PurchaseSort.LowToHighCountBuying:
                sortedQuery = query.OrderBy(x => x.PurchaseOrderDetails.Count).ToList();
                break;
            case PurchaseSort.HighToLowCountBuying:
                sortedQuery = query.OrderByDescending(x => x.PurchaseOrderDetails.Count).ToList();
                break;
            case PurchaseSort.LowToHighPiceBuying:
                sortedQuery = query.OrderBy(x => x.Amount).ToList();
                break;
            case PurchaseSort.HighToLowPriceBuying:
                sortedQuery = query.OrderByDescending(x => x.Amount).ToList();
                break;
        }

        var purchaseList = await query.Select(p => new PurchaseListViewModel
        {
            Id = p.Id,
          Amount=p.Amount,
          CreationDate=p.CreationDate,
          IsPaied=p.IsPaid,
          Description =p.Description
        }).ToListAsync(cancellationToken);

        return PagedList<PurchaseListViewModel>.ToPagedList(purchaseList,
            purchaseFiltreOrderViewModel.PaginationParameters.PageNumber,
            purchaseFiltreOrderViewModel.PaginationParameters.PageSize);
    }

    public async Task<PurchaseOrder?> GetByUser(int id, CancellationToken cancellationToken) => await _context.PurchaseOrders.Where(x => x.UserId == id && !x.IsPaid).Include(x => x.PurchaseOrderDetails).Include(a => a.SendInformation)
            .FirstOrDefaultAsync(cancellationToken);
    public async Task<PurchaseOrder?> GetByOrderId(long id, CancellationToken cancellationToken) => await _context.PurchaseOrders.Where(x => x.OrderId == id && !x.IsPaid).Include(x => x.PurchaseOrderDetails).Include(a => a.SendInformation)
           .FirstOrDefaultAsync(cancellationToken);
    public async Task<IEnumerable<PurchaseOrderViewModel>> GetProductListByUserId(int userId,
        CancellationToken cancellationToken)
    {
        var purchaseOrderViewModel = await _context.PurchaseOrderDetails
            .Where(x => x.PurchaseOrder!.UserId == userId && !x.PurchaseOrder.IsPaid &&
                        x.PurchaseOrder.Status == Status.New)
            .Select(p => new PurchaseOrderViewModel
            {
                Id = p.Id,
                ProductId = p.ProductId,
                Url = p.Product.Url,
                Name = p.Product.Name,
                Price = p.Product.Prices!.First(x => x.Id == p.PriceId),
                PriceAmount = p.Product.Prices!.First(x => x.Id == p.PriceId).Amount,
                PriceId = p.PriceId,
                ImagePath = $"{p.Product.Images!.FirstOrDefault()!.Path}/{p.Product.Images!.FirstOrDefault()!.Name}",
                Brand = p.Product.Brand!.Name,
                Alt = p.Product.Images!.FirstOrDefault()!.Alt,
                //Exist = p.Product.Exist,
                IsColleague = p.PurchaseOrder!.User!.IsColleague,
                UserId = p.PurchaseOrder.UserId,
                Quantity = p.Quantity,
                SumPrice = p.Quantity * p.Product.Prices!.First(x => x.Id == p.PriceId).Amount,
                ColorName = p.Product.Prices!.First(x => x.Id == p.PriceId).Color.Name
            })
            .ToListAsync(cancellationToken);
        return purchaseOrderViewModel;
    }
}