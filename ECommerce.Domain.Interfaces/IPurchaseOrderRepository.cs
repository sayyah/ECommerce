using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IPurchaseOrderRepository : IRepositoryBase<PurchaseOrder>
{
    PagedList<PurchaseListViewModel> Search(PurchaseFilterOrderViewModel purchaseFilterOrderViewModel);

    Task<PurchaseOrder?> GetByUser(int id, Status status, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByOrderId(long id, CancellationToken cancellationToken);
    Task<List<PurchaseOrderViewModel>?> GetProductListByUserId(int userId, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByOrderIdWithInclude(long orderId, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetPurchaseOrderWithIncludeById(int id, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByUserAndOrderId(int userId, long orderId, CancellationToken cancellationToken);
}
