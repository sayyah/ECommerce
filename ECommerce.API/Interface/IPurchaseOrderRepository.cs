﻿using API.Utilities;
using Entities;
using Entities.Helper;
using Entities.ViewModel;

namespace API.Interface;

public interface IPurchaseOrderRepository : IAsyncRepository<PurchaseOrder>
{
    Task<PagedList<PurchaseListViewModel>> Search(PurchaseFiltreOrderViewModel purchaseFiltreOrderViewModel,
        CancellationToken cancellationToken);

    Task<PurchaseOrder?> GetByUser(int id, CancellationToken cancellationToken);
    Task<PurchaseOrder?> GetByOrderId(long id, CancellationToken cancellationToken);

    Task<IEnumerable<PurchaseOrderViewModel>> GetProductListByUserId(int userId, CancellationToken cancellationToken);
}