using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void GetByIdAsync_NullArgument_ReturnsNull()
    {
        // Act
        Category? actual = await _categoryRepository.GetByIdAsync(CancellationToken, null!);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async void GetByIdAsync_GetEntityById_ReturnsEntity()
    {
        // Arrange
        var parent = Fixture
            .Build<Category>()
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();
        var children = Fixture
            .Build<Category>()
            .With(p => p.Parent, () => parent)
            .With(p => p.ParentId, () => parent.Id)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .CreateMany(5);
        parent.Categories = children.ToList();
        var categories = children.Append(parent);
        DbContext.Categories.AddRange(categories);
        DbContext.SaveChanges();

        Category expectedCategory = categories.ElementAt(2);

        // Act
        Category? actual = await _categoryRepository.GetByIdAsync(
            CancellationToken,
            expectedCategory.Id
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }
}
