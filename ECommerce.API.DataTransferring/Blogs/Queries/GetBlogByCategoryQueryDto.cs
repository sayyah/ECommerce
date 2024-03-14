﻿using ECommerce.Application.Services.Objects;

namespace ECommerce.API.DataTransferObject.Blogs.Queries
{
    public class GetBlogByCategoryQueryDto
    {
        public PaginationParameters PaginationParameters { get; set; } = new();
        public int CategoryId { get; set; }
    }
}