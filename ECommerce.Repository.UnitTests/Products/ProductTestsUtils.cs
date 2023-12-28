using AutoFixture;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using ECommerce.Repository.UnitTests.Base.Customizations;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public partial class ProductTests : BaseTests
{
    private readonly IProductRepository _productRepository;

    public ProductTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        Fixture.Customize(new DiscountCustomization());
        Fixture.Customize(new ColorCustomization());
        Fixture.Customize(new PriceCustomization());
        Fixture.Customize(new ImageCustomization());
        Fixture.Customize(new BrandCustomization());
        Fixture.Customize(new KeywordCustomization());
        Fixture.Customize(new TagCustomization());
        Fixture.Customize(new ProductCustomization());
    }
}
