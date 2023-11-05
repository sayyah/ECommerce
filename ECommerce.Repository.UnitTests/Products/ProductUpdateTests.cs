using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public class ProductUpdateTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly Dictionary<string, Dictionary<string, Product>> _testSets;

    public ProductUpdateTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);
        _testSets = ProductTestUtils.GetTestSets();
    }

    [Fact(DisplayName = "Update: Null input")]
    public void Update_NullInput_ThrowsException()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["null_test"];

        // Act
        void actual() => _productRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "Update: Update product")]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, Product> expected = _testSets["unique_url"];
        DbContext.Products.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        Product productToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
        Product expectedProduct = DbContext.Products.Single(p => p.Id == productToUpdate.Id)!;
        expectedProduct.Url = Guid.NewGuid().ToString();
        expectedProduct.Name = Guid.NewGuid().ToString();
        expectedProduct.MinOrder = Random.Shared.Next();

        // Act
        _productRepository.Update(expectedProduct);

        // Assert
        Product? actual = DbContext.Products.Single(p => p.Id == productToUpdate.Id);
        Assert.Equivalent(expectedProduct, actual);
    }
}
