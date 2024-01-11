using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction>
{
    Task<PagedList<Transaction>> Search(transactionFilterViewModel transactionFilterViewModel,
        CancellationToken cancellationToken);
}
