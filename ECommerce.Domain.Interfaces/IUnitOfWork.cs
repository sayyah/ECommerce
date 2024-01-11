using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    TRepository GetRepository<TRepository, TEntity>() where TRepository : class, IRepositoryBase<TEntity> where TEntity : class, IBaseEntity<int>;
    TRepository GetHolooRepository<TRepository, TEntity>() where TRepository : class, IHolooRepository<TEntity> where TEntity : BaseHolooEntity;
    Task SaveAsync(CancellationToken cancellationToken, bool isHolooChange = false);
}
