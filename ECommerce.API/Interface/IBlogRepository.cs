﻿using Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Utilities;
using Entities.Helper;
using Entities.ViewModel;

namespace API.Interface
{
    public interface IBlogRepository : IAsyncRepository<Blog>
    {
        Task<PagedList<Blog>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
        Task<Blog> GetByTitle(string title, CancellationToken cancellationToken);

        Task<Blog> AddWithRelations(BlogViewModel blogViewModel, CancellationToken cancellationToken);

        Task<Blog> EditWithRelations(BlogViewModel blogViewModel, CancellationToken cancellationToken);

        Task<IEnumerable<Blog>> GetWithInclude(int id, CancellationToken cancellationToken);

        Task<Blog> GetByUrl(string url, CancellationToken cancellationToken);
    }
}