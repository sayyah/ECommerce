using ECommerce.Infrastructure.Repository;
using System.Reflection;

namespace ECommerce.API;

public static class IServiceCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services, Assembly assembly)
    {
        var repositoryTypes = assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRepositoryBase<>)));

        // filter out RepositoryBase<>
        var nonBaseRepos = repositoryTypes.Where(t => t != typeof(RepositoryBase<>));

        foreach (var repositoryType in nonBaseRepos)
        {
            var interfaces = repositoryType.GetInterfaces()
                .Where(@interface => @interface.IsGenericType 
                                     && @interface.GetGenericTypeDefinition() == typeof(IRepositoryBase<>)
                                     && @interface.GetGenericTypeDefinition() == typeof(IHolooRepository<>))
                .ToList();

            if (interfaces.Count != 1)
            {
                throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IRepositoryBase<T>.");
            }

            services.AddScoped(interfaces[0], repositoryType);
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
