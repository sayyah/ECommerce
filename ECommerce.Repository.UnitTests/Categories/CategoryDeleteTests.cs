using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void Delete_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _categoryRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Delete_DeleteEntity_EntityRemovedFromRepository()
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
        Category categoryToRemove = categories.ElementAt(2);

        // Act
        _categoryRepository.Delete(categoryToRemove);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(categories.Count() - 1, DbContext.Categories.Count());
        Exception exception = Assert.Throws<InvalidOperationException>(
            () => DbContext.Categories.Single(c => c.Id == categoryToRemove.Id)
        );
        Assert.Equal("Sequence contains no elements", exception.Message);
        var withoutDeletedCategory = categories.Where(c => c.Id != categoryToRemove.Id);
        DbContext.Categories.Should().BeEquivalentTo(withoutDeletedCategory);
    }
}
