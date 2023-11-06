using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ITransactionRepository : IAsyncRepository<Transaction>
{
    Task<PagedList<Transaction>> Search(TransactionFiltreViewModel transactionFiltreViewModel,
        CancellationToken cancellationToken);
}
