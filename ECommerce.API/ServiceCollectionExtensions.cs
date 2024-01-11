using System.Reflection;
using ECommerce.Infrastructure.Base.Ioc;

namespace ECommerce.API;

public static class ServiceCollectionExtensions
{
    public static void ExecuteRegistratorsFromAssemblyOf<TRegistrator>(this IServiceCollection services)
        where TRegistrator : IServiceCollectionRegistrar
    {
        IEnumerable<Type> registratorTypes = typeof(TRegistrator).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IServiceCollectionRegistrar).IsAssignableFrom(t));
        foreach (Type registratorType in registratorTypes)
        {
            var registrator = (IServiceCollectionRegistrar)Activator.CreateInstance(registratorType);
            registrator.RegisterIn(services);
        }
    }

    public static void AddRepositories(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}

