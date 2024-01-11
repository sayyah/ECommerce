using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ITransactionRepository : IRepositoryBase<Transaction>
{
    PagedList<Transaction> Search(transactionFilterViewModel transactionFilterViewModel);
}
