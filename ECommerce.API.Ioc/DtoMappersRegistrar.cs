using ECommerce.API.DataTransferObjectMappers;
using ECommerce.Infrastructure.Base.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Ioc;
public class DtoMappersRegistrar : IServiceCollectionRegistrar
{
    public void RegisterIn(IServiceCollection services)
    {
        //install AutoMapper.Extensions.Microsoft.DependencyInjection NuGet Package
        services.AddAutoMapper(typeof(DtoMap));
    }
}
