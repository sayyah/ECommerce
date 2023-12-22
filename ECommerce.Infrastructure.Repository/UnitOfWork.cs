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
        var repositoryInstance = repositoryType.GetConstructors()[0].GetParameters().Length > 1 ?
            Activator.CreateInstance(repositoryType, context, holooContext):
        Activator.CreateInstance(repositoryType, context);
        if (repositoryInstance != null) _repositories.Add(type, repositoryInstance);

        return (TRepository)_repositories[type];
    }

    TRepository IUnitOfWork.GetHolooRepository<TRepository, TEntity>()
    {
        var type = typeof(TEntity);

        if (_holooRepositories.TryGetValue(type, out var repository))
        {
            return (TRepository)repository;
        }

        var repositoryType = typeof(TRepository);
        var repositoryInstance = Activator.CreateInstance(repositoryType, holooContext);
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
