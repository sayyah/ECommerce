using ECommerce.API.DataTransferObjectMappers;
using ECommerce.Infrastructure.Base.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Ioc;
public class DtoMappersRegistrar : IServiceCollectionRegistrar
{
    public void RegisterIn(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DtoMap));
    }
}
