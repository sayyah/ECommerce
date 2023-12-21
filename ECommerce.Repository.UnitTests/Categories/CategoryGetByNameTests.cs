using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void GetByName_GetEntityByName_ReturnsEntity()
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

        Category expectedCategory = categories.ElementAt(3);

        // Act
        Category? actual = await _categoryRepository.GetByName(
            expectedCategory.Name,
            CancellationToken,
            expectedCategory.ParentId
        );

        // Assert
        actual.Should().BeEquivalentTo(expectedCategory);
    }

    [Fact]
    public async void GetByName_NonExistingName_ReturnsNull()
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

        Category expectedCategory = categories.ElementAt(3);

        // Act
        Category? actual = await _categoryRepository.GetByName(
            Fixture.Create<string>(),
            CancellationToken,
            expectedCategory.ParentId
        );

        // Assert
        actual.Should().BeNull();
    }
}
