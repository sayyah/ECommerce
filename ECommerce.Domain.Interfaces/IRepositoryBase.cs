using ECommerce.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Domain.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : class, IBaseEntity<int>
{
    DbSet<TEntity> Entities { get; }
    IQueryable<TEntity> Table { get; }
    IQueryable<TEntity> TableNoTracking { get; }


    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    IQueryable<TEntity> GetAll(string includeProperties);
    TEntity? GetByIdWithInclude(string includeProperties, int ids);
    Task<TEntity?> GetByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, int? id);

    Task<IEnumerable<TEntity>?> Where(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken);

    void Add(TEntity entity);
    Task<TEntity> AddAsync(TEntity entity,CancellationToken cancellationToken);
    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    Task DeleteById(int id, CancellationToken cancellationToken);
    void DeleteRange(IEnumerable<TEntity> entities);

    Task LoadCollectionAsync<TProperty>(TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
        where TProperty : class;

    Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty,
        CancellationToken cancellationToken) where TProperty : class;

    void Attach(TEntity entity);
    void Detach(TEntity entity);
}
