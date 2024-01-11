using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Base.Ioc;
public interface IServiceCollectionRegistrar
    {
        void RegisterIn(IServiceCollection services);
    }
