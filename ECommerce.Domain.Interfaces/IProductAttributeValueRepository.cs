﻿using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IProductAttributeValueRepository : IAsyncRepository<ProductAttributeValue>
{
    Task<PagedList<ProductAttributeValue>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);
}