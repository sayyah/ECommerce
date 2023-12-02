using Ecommerce.Entities.ViewModel;

namespace ECommerce.Infrastructure.Repository;

public class DiscountRepository(SunflowerECommerceDbContext context) : AsyncRepository<Discount>(context),
    IDiscountRepository
{
    public async Task<Discount> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Discounts.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Discount?> GetLast(CancellationToken cancellationToken)
    {
        var result = await context.Discounts.Include(i => i.Prices).ThenInclude(x => x.Product)
            .ThenInclude(y => y.Images).OrderByDescending(o => o.EndDate).FirstOrDefaultAsync(cancellationToken);
        if (result.Prices.Count() > 0) return result;
        return null;
    }


    public async Task<Discount> AddWithRelations(DiscountViewModel discountViewModel, CancellationToken cancellationToken)
    {
        Discount discount = discountViewModel;
        discount.Categories = new List<Category>();
        foreach (var id in discountViewModel.CategoriesId) discount.Categories.Add(await context.Categories.FindAsync(id));
        discount.Prices = new List<Price>();
        foreach (var id in discountViewModel.PricesId) discount.Prices.Add(await context.Prices.FindAsync(id));
        await context.Discounts.AddAsync(discount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return discount;
    }

    public async Task<Discount> GetByCode(string code, CancellationToken cancellationToken)
    {
        return await context.Discounts.Where(x => x.Code == code).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DiscountWithTimeViewModel> GetWithTime(CancellationToken cancellationToken)
    {
        var discount = await context.Discounts.Where(x => x.EndDate < DateTime.Now).Include(x => x.Prices)
            .FirstOrDefaultAsync(cancellationToken);
        var product = new Product();
        if (discount == null)
        {
            product = await context.Products
                .Include(x => x.Images)
                .Include(x => x.Brand)
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            //var temp = discount.Products.OrderByDescending(x => x.Prices.Max(x => x.Amount)).FirstOrDefault();
            var temp = discount.Prices.Select(x => x.Product).OrderByDescending(x => x.Prices.Max(y => y.Amount))
                .FirstOrDefault();
            product = await context.Products
                .Where(x => x.Id == temp.Id)
                .Include(x => x.Images)
                .Include(x => x.Brand)
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var ret = new DiscountWithTimeViewModel
        {
            ProductId = product.Id,
            Alt = product.Images.FirstOrDefault()?.Alt,
            Brand = product.Brand.Name,
            Description = product.Description,
            EndDateTime = discount?.EndDate,
            Price = product.Prices.FirstOrDefault(x => !x.IsColleague && x.MinQuantity == 1).Amount,
            ImagePath = $"{product.Images.FirstOrDefault()?.Path}/{product.Images.FirstOrDefault()?.Name}",
            Name = product.Name,
            Url = product.Url
        };
        return ret;
    }

    public bool Active(int id)
    {
        var discount = context.Discounts.Find(id);
        discount.IsActive = !discount.IsActive;
        Update(discount);
        return discount.IsActive;
    }

    public async Task<PagedList<Discount>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Discount>.ToPagedList(
            await context.Discounts.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
