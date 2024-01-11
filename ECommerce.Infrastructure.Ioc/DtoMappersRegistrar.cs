using ECommerce.Infrastructure.Base.Ioc;
using ECommerce.Infrastructure.DataTransferObjectMappers;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Ioc;
public class DtoMappersRegistrar : IServiceCollectionRegistrar
{
    public void RegisterIn(IServiceCollection services)
    {
        //install AutoMapper.Extensions.Microsoft.DependencyInjection NuGet Package
        services.AddAutoMapper(typeof(DtoMap));
    }
}
