using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _categoryRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void DeleteRange_DeleteEntities_EntitiesRemovedFromRepository()
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
        var categoryNotToRemove = categories.ElementAt(2);
        IEnumerable<Category> categoriesToRemove = categories.Where(
            c => c.Id != categoryNotToRemove.Id
        );

        // Act
        _categoryRepository.DeleteRange(categoriesToRemove);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Categories.Count().Should().Be(1);
        DbContext.Categories.Should().ContainEquivalentOf(categoryNotToRemove);
    }
}
