using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IPurchaseOrderDetailRepository : IAsyncRepository<PurchaseOrderDetail>
{
    Task<List<PurchaseOrderDetail>> GetByPurchaseOrderId(int id, CancellationToken cancellationToken);
    Task UpdateUserCart(IEnumerable<PurchaseOrderViewModel> purchaseOrderList, CancellationToken cancellationToken);
}
