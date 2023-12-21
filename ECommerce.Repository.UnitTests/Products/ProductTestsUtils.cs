using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public partial class ProductTests : BaseTests
{
    private readonly IProductRepository _productRepository;

    public ProductTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
    }
}
