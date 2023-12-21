using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void Update_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _categoryRepository.Update(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Update_UpdateEntity_EntityChanges()
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
        Category categoryToUpdate = expected.ElementAt(3);

        categoryToUpdate.Name = Guid.NewGuid().ToString();

        // Act
        _categoryRepository.Update(categoryToUpdate);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected);
    }
}
