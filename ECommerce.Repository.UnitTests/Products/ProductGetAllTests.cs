using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

public partial class ProductTests
{
    [Fact]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.CreateMany<Product>(2).ToList();
        DbContext.Products.AddRange(expected);
        DbContext.SaveChanges();

        // Act
        var actuals = _productRepository.GetAll("");

        // Assert
        actuals.Should().BeEquivalentTo(expected);
    }
}
