using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IDiscountRepository : IRepositoryBase<Discount>
{
    PagedList<Discount> Search(PaginationParameters paginationParameters);

    Task<Discount?> GetByName(string name, CancellationToken cancellationToken);

    Task<Discount?> GetByCode(string code, CancellationToken cancellationToken);

    Task<Discount?> GetLast(CancellationToken cancellationToken);

    Task<DiscountWithTimeViewModel?> GetWithTime(CancellationToken cancellationToken);

    bool Active(int id);
}
