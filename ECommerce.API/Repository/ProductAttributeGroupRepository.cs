﻿using API.DataContext;
using API.Interface;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Utilities;
using Entities.Helper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class ProductAttributeGroupRepository : AsyncRepository<ProductAttributeGroup>, IProductAttributeGroupRepository
    {
        private readonly SunflowerECommerceDbContext _context;
        public ProductAttributeGroupRepository(SunflowerECommerceDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PagedList<ProductAttributeGroup>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken)
        {
            return PagedList<ProductAttributeGroup>.ToPagedList(await _context.ProductAttributeGroups.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking().OrderBy(on => on.Id).ToListAsync(cancellationToken),
                paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }
        public async Task<ProductAttributeGroup> GetByName(string name, CancellationToken cancellationToken) => await _context.ProductAttributeGroups.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);

        public async Task<List<ProductAttributeGroup>> GetWithInclude(CancellationToken cancellationToken)
        {
            return await _context.ProductAttributeGroups.
                Include(nameof(ProductAttributeGroup.Attribute)).
                ToListAsync(cancellationToken); ;
        }
        public async Task<IEnumerable<ProductAttributeGroup>> GetAllAttributeWithProductId(int productId, CancellationToken cancellationToken)
        {
            var group = await _context.ProductAttributeGroups
                //.Where(p => p.Products.Any(x => x.Id == productId))
                .Include(a => a.Attribute)
                .ToListAsync(cancellationToken);
            var productValues = await _context.ProductAttributeValues.Where(x => x.ProductId == productId).ToListAsync(cancellationToken);

            foreach (var productAttributeGroup in group)
            {
                foreach (var attribute in productAttributeGroup.Attribute)
                {
                    if (attribute.AttributeValue.Count == 0)
                    {
                        attribute.AttributeValue.Add(new ProductAttributeValue());
                    }
                }

            }

            return group;
        }

        public async Task<List<ProductAttributeGroup>> AddWithAttributeValue(List<ProductAttributeGroup> productAttributeGroups, int productId, CancellationToken cancellationToken)
        {
            foreach (var productAttributeGroup in productAttributeGroups)
            {
                foreach (var productAttribute in productAttributeGroup.Attribute)
                {
                    if (productAttribute.AttributeValue[0].Id > 0)
                    {
                        var entity =
                            _context.ProductAttributeValues.First(x => x.Id == productAttribute.AttributeValue[0].Id);
                        entity.Value = productAttribute.AttributeValue[0].Value;
                        _context.ProductAttributeValues.Update(entity);
                    }
                    else
                    {
                        if (productAttribute.AttributeValue[0].Value != null)
                        {
                            _context.ProductAttributeValues.Add(new ProductAttributeValue
                            {
                                ProductId = productId,
                                Value = productAttribute.AttributeValue[0].Value,
                                ProductAttributeId = productAttribute.Id
                            });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return productAttributeGroups;
        }
    }
}