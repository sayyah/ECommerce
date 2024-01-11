﻿using ECommerce.Infrastructure.Base.Ioc;
using ECommerce.Infrastructure.Handlers.Blogs.Commands;
using ECommerce.Infrastructure.Handlers.Blogs.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Ioc
{
    public class QueryHandlerRegistrar : IServiceCollectionRegistrar
    {
        public void RegisterIn(IServiceCollection services)
        {
            services.RegisterQueryHandlersFromAssemblyOf<GetBlogsQueryHandler>();
            services.RegisterCommandHandlersFromAssemblyOf<CreateBlogCommandHandler>();
        }
    }
}
