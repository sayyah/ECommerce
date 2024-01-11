using ECommerce.Infrastructure.Base.Ioc;
using ECommerce.Infrastructure.Handlers.Blogs.Commands;
using ECommerce.Infrastructure.Handlers.Blogs.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Ioc
{
    public class CommandHandlerRegistrar : IServiceCollectionRegistrar
    {
        public void RegisterIn(IServiceCollection services)
        {
            services.RegisterCommandHandlersFromAssemblyOf<CreateBlogCommandHandler>();
        }
    }
}
