using ECommerce.Infrastructure.Base.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Ioc
{
    public class UnitOfWorkRegistrar : IServiceCollectionRegistrar
    {
        public void RegisterIn(IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
