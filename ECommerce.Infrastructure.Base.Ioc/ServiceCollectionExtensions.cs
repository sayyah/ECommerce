using ECommerce.Application.Base.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Base.Ioc;
public static class ServiceCollectionExtensions
{
    public static void RegisterQueryHandlersFromAssemblyOf<TImplementation>(this IServiceCollection services)
    {
        services.RegisterAllAssignableToAnyOfTypesFromAssemblyOf<TImplementation>(
            ServiceLifetime.Transient,
            typeof(IQueryHandler<,>));

        //services.DecorateIfHasAny(typeof(IQueryHandler<,>), typeof(ValidatedQueryHandler<,>));
    }

    private static void RegisterAllAssignableToAnyOfTypesFromAssemblyOf<TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        params Type[] types)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TImplementation>()
            .AddClasses(classes => classes.AssignableToAny(types))
            .AsImplementedInterfaces()
            .WithLifetime(lifetime));
    }

    private static void DecorateIfHasAny(
        this IServiceCollection services,
        Type serviceType,
        params Type[] decoratorTypes)
    {
        if (services.HasAnyServiceTypeAssignableFrom(serviceType))
        {
            services.Decorate(serviceType, decoratorTypes);
        }
    }

    private static void Decorate(
        this IServiceCollection services,
        Type serviceType,
        IEnumerable<Type> decoratorTypes)
    {
        foreach (Type decoratorType in decoratorTypes)
        {
            services.Decorate(serviceType, decoratorType);
        }
    }

    private static bool HasAnyServiceTypeAssignableFrom(
        this IServiceCollection services,
        Type type)
    {
        bool hasAny;
        if (type.IsGenericType)
        {
            hasAny = services.Any(sd => sd.ServiceType != null
                                        && sd.ServiceType.IsGenericType
                                        && type.IsAssignableFrom(sd.ServiceType.GetGenericTypeDefinition()));
        }
        else
        {
            hasAny = services.Any(sd => sd.ServiceType != null
                                        && type.IsAssignableFrom(sd.ServiceType));
        }
        return hasAny;
    }
}

