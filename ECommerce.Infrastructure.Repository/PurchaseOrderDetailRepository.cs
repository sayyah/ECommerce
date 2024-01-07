namespace ECommerce.Infrastructure.Repository;

public class PurchaseOrderDetailRepository
    (SunflowerECommerceDbContext context) : AsyncRepository<PurchaseOrderDetail>(context),
        IPurchaseOrderDetailRepository
{
    public async Task<List<PurchaseOrderDetail>> GetByPurchaseOrderId(int id, CancellationToken cancellationToken)
    {
        return await context.PurchaseOrderDetails
            .Where(x => x.PurchaseOrderId == id)
            .Include(x => x.Price)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateUserCart(IEnumerable<PurchaseOrderViewModel> purchaseOrderList,
        CancellationToken cancellationToken)
    {
        List<PurchaseOrderDetail> purchaseOrders = new();
        foreach (var purchaseOrderViewModel in purchaseOrderList)
        {
            var purchaseOrderDetail = await GetByIdAsync(cancellationToken, purchaseOrderViewModel.Id);
            purchaseOrderDetail.UnitPrice = purchaseOrderViewModel.PriceAmount;
            purchaseOrderDetail.SumPrice = purchaseOrderViewModel.SumPrice;
            purchaseOrders.Add(purchaseOrderDetail);
        }

        await UpdateRangeAsync(purchaseOrders, cancellationToken);
    }
}
