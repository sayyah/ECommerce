using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class UnitOfWork(SunflowerECommerceDbContext context, HolooDbContext holooContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly Dictionary<Type, object> _holooRepositories = new();

    public void Dispose()
    {
        context.Dispose();
    }

    TRepository IUnitOfWork.GetRepository<TRepository, TEntity>()
    {
        var type = typeof(TEntity);

        if (_repositories.TryGetValue(type, out var repository))
        {
            return (TRepository)repository;
        }

        var repositoryType = typeof(TRepository);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);
        if (repositoryInstance != null) _repositories.Add(type, repositoryInstance);

        return (TRepository)_repositories[type];
    }

    public TRepository GetHolooRepository<TRepository, TEntity>() where TRepository : class, IHolooRepository<TEntity> where TEntity : BaseHolooEntity
    {
        var type = typeof(TEntity);

        if (_holooRepositories.TryGetValue(type, out var repository))
        {
            return (TRepository)repository;
        }

        var repositoryType = typeof(TRepository);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), holooContext);
        if (repositoryInstance != null) _holooRepositories.Add(type, repositoryInstance);

        return (TRepository)_holooRepositories[type];
    }

    public async Task SaveAsync(CancellationToken cancellationToken, bool isHolooChange = false)
    {
        await context.SaveChangesAsync(cancellationToken);
        if (isHolooChange)
        {
           await holooContext.SaveChangesAsync(cancellationToken);
        }
    }
}
