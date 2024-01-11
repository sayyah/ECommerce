using ECommerce.Infrastructure.DataContext.Utilities;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repository;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IBaseEntity<int>
{
    protected SunflowerECommerceDbContext DbContext;

    public RepositoryBase(SunflowerECommerceDbContext dbContext)
    {
        DbContext = dbContext;
        Entities = DbContext.Set<TEntity>();
    }

    public DbSet<TEntity> Entities { get; }
    public virtual IQueryable<TEntity> Table => Entities;
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    #region Get

    public IQueryable<TEntity> GetAll(string? includeProperties)
    {
        IQueryable<TEntity> query = Entities;

        if (string.IsNullOrEmpty(includeProperties) || string.IsNullOrWhiteSpace(includeProperties)) return query;

        return includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public virtual TEntity? GetByIdWithInclude(string includeProperties, int ids)
    {
        IQueryable<TEntity> query = Entities;
        query = query.Where(x => x.Id == ids);

        if (string.IsNullOrWhiteSpace(includeProperties) || string.IsNullOrEmpty(includeProperties))
            return null;
        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return query.FirstOrDefault();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Entities.AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return await Entities.FindAsync(ids, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>?> Where(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await Entities.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, int? id)
    {
        return await Entities.FindAsync(id, cancellationToken);
    }

    #endregion

    #region Add

    public virtual void Add(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Add(entity);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Assert.NotNull(entity, nameof(entity));
        var entityEntry = await Entities.AddAsync(entity, cancellationToken);
        return entityEntry.Entity;
    }


    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        var baseEntities = entities as TEntity[] ?? entities.ToArray();
        Assert.NotNull(baseEntities, nameof(entities));
        Entities.AddRange(baseEntities);
    }

    #endregion

    #region Update

    public virtual void Update(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        var baseEntities = entities as TEntity[] ?? entities.ToArray();
        Assert.NotNull(baseEntities, nameof(entities));
        Entities.UpdateRange(baseEntities);
    }

    #endregion

    #region Delete

    public virtual void Delete(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        Entities.Remove(entity);


    }

    public virtual async Task DeleteById(int id, CancellationToken cancellationToken)
    {
        var entity = await Entities.FindAsync(id, cancellationToken);
        if (entity != null) Entities.Remove(entity);
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        var baseEntities = entities as TEntity[] ?? entities.ToArray();
        Assert.NotNull(baseEntities, nameof(entities));
        Entities.RemoveRange(baseEntities);
    }

    #endregion

    #region Explicit Loading

    public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);

        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded) await collection.LoadAsync(cancellationToken);
    }

    public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity,
        Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);
        var reference = DbContext.Entry(entity).Reference(referenceProperty!);
        if (!reference.IsLoaded) await reference.LoadAsync(cancellationToken);
    }

    #endregion

    #region Attach & Detach

    public virtual void Detach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        var entry = DbContext.Entry(entity);
        entry.State = EntityState.Detached;
    }

    public virtual void Attach(TEntity entity)
    {
        Assert.NotNull(entity, nameof(entity));
        if (DbContext.Entry(entity).State == EntityState.Detached) Entities.Attach(entity);
    }

    #endregion
}
