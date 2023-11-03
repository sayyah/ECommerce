﻿using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IImageRepository : IAsyncRepository<Image>
{
    Task<PagedList<Image>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<int> DeleteByName(string name, CancellationToken cancellationToken);
    Task<List<Image>> GetByProductId(int productId, CancellationToken cancellationToken);
    Task<Image> GetByBlogId(int blogId, CancellationToken cancellationToken);
}