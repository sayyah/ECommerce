using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void Action() => _categoryRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public async void UpdateRange_UpdateEntities_EntitiesChange()
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
        var expected = children.Append(parent);
        DbContext.Categories.AddRange(expected);
        DbContext.SaveChanges();
        var categoryToNotChange = expected.ElementAt(3);
        var categoriesToUpdate = expected.Where(c => c.Id != categoryToNotChange.Id);

        foreach (var item in categoriesToUpdate)
        {
            item.Name = Guid.NewGuid().ToString();
        }

        // Act
        _categoryRepository.UpdateRange(categoriesToUpdate);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected);
    }
}
