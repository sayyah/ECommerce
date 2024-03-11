using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class CategoryRepository(SunflowerECommerceDbContext context) : RepositoryBase<Category>(context),
    ICategoryRepository
{
    public async Task<Category> GetByName(string name, CancellationToken cancellationToken, int? parentId = null)
    {
        return await context.Categories
            .Where(x => x.Name == name && x.ParentId == parentId && x.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Category>?> Search(string searchKeyword, CancellationToken cancellationToken)
    {
        return await context.Categories
            .Where(x => x.Name.Contains(searchKeyword) && x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAll(IEnumerable<Category> categories, CancellationToken cancellationToken)
    {
        await context.Categories.AddRangeAsync(categories, cancellationToken);
    }

    public async Task<IEnumerable<int>> GetIdsByUrl(string categoryUrl, CancellationToken cancellationToken)
    {
        return await context.Categories
            .Where(x => (x.Path == categoryUrl || x.Categories.Any(y => y.Path == categoryUrl)) && x.IsActive)
            .Select(x => x.Id).ToListAsync(cancellationToken);
    }

    public async Task<CategoryViewModel?> GetByUrl(string categoryUrl, CancellationToken cancellationToken)
    {
        CategoryViewModel? categoryViewModel = await context.Categories
            //.Where(x => (x.Path == categoryUrl || x.Categories.Any(y => y.Path == categoryUrl)) && x.IsActive)
            .Where(x => x.Path == categoryUrl && x.IsActive)
            .Select(x => new CategoryViewModel
            {
                Name = x.Name,
                Id = x.Id,
                Categories = x.Categories.Select(c => c.Id).ToList(),
                Parent = x.Parent,
                ParentId = x.ParentId,
                ProductsId = x.Products.Select(p => p.Id).ToList(),
                ImagePath = x.ImagePath
            }).FirstOrDefaultAsync(cancellationToken);
        if (categoryViewModel != null)
        {
            int? categoryParentId = categoryViewModel.ParentId;
            while (string.IsNullOrEmpty(categoryViewModel.ImagePath) && categoryParentId != null)
            {
                Category? parent = await context.Categories.FirstOrDefaultAsync(x => x.Id == categoryParentId, cancellationToken: cancellationToken);
                if (parent == null)
                {
                    categoryParentId = null;
                    continue;
                }
                categoryViewModel.ImagePath = parent.ImagePath;
                categoryParentId = parent.ParentId;
            }
        }
        return categoryViewModel;
    }

    public async Task<List<CategoryParentViewModel>?> Parents(int productId, CancellationToken cancellationToken)
    {
        var productCategory = new List<int>();
        if (productId > 0)
        {
            var temp = context.Products.Where(x => x.Id == productId).Include(x => x.ProductCategories);
            productCategory = temp.First().ProductCategories.Select(x => x.Id).ToList();
        }

        var allCategory = await context.Categories.Where(x => x.IsActive).Include(x=>x.Discount).ToListAsync(cancellationToken);

        var result = await Children(allCategory, productCategory, null);
        return result.OrderBy(x => x.DisplayOrder).ToList();
    }

    public async Task<List<int>> ChildrenCategory(int categoryId, CancellationToken cancellationToken)
    {
        var categoriesId = new List<int>();

        var categories = context.Categories.Where(x => x.ParentId == categoryId);
        if (categories.Any())
            foreach (var i in categories.Select(x => x.Id))
            {
                categoriesId.Add(i);
                categoriesId.AddRange(await ChildrenCategory(i, cancellationToken));
            }
        else
            categoriesId.Add(categoryId);

        return categoriesId;
    }

    public async Task<PagedList<Category>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Category>.ToPagedList(
            await context.Categories.Where(x => x.Name.Contains(paginationParameters.Search)).OrderBy(on => on.Id)
                .ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public IQueryable<Category> GetCategoriesWithProduct(CancellationToken cancellationToken)
    {
        return context.Categories.Include(x => x.Products);
    }

    private async Task<List<CategoryParentViewModel>> Children(List<Category> allCategory,
        List<int> productCategory, int? parentId)
    {
        var temp = new List<CategoryParentViewModel>();
        var ret = new List<CategoryParentViewModel>();
        foreach (var parent in allCategory.Where(p => p.ParentId == parentId).OrderBy(x => x.DisplayOrder).ToList())
        {
            if (allCategory.Any(p => p.ParentId == parent.Id))
                temp = await Children(allCategory, productCategory, parent.Id);

            ret.Add(new CategoryParentViewModel
            {
                Id = parent.Id,
                Name = parent.Name,
                Path = parent.Path,
                Depth = parent.Depth,
                Children = temp,
                Checked = productCategory.Contains(parent.Id),
                DisplayOrder = parent.DisplayOrder,
                Discount = parent.Discount                
            });
            temp = new List<CategoryParentViewModel>();
        }

        return ret;
    }
}
