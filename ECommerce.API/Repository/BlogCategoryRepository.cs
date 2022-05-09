﻿using API.DataContext;
using API.Interface;
using Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class BlogCategoryRepository : AsyncRepository<BlogCategory>, IBlogCategoryRepository
    {
        private readonly SunflowerECommerceDbContext _context;
        public BlogCategoryRepository(SunflowerECommerceDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<BlogCategory> GetByName(string name,int? parentId, CancellationToken cancellationToken) => await _context.BlogCategories.Where(x => x.Name == name&& x.Parent.Id==parentId).FirstOrDefaultAsync(cancellationToken);

    }
}