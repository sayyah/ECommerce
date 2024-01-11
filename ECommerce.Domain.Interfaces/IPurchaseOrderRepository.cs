using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IPurchaseOrderRepository : IRepositoryBase<PurchaseOrder>
{
    Task<PagedList<PurchaseListViewModel>> Search(PurchaseFiltreOrderViewModel purchaseFiltreOrderViewModel,
        CancellationToken cancellationToken);

    Task<PurchaseOrder?> GetByUser(int id, Status status, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByOrderId(long id, CancellationToken cancellationToken);
    Task<List<PurchaseOrderViewModel>?> GetProductListByUserId(int userId, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByOrderIdWithInclude(long orderId, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetPurchaseOrderWithIncludeById(int id, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByUserAndOrderId(int userId, long orderId, CancellationToken cancellationToken);
}
