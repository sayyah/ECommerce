using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async Task AddRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _categoryRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task AddRange_RequiredNameField_ThrowsException()
    {
        // Arrange
        IEnumerable<Category> category =
        [
            Fixture
            .Build<Category>()
            .With(p => p.Name, () => null!)
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create()
        ];

        // Act
        async Task Actual()
        {
            _categoryRepository.AddRange(category);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact]
    public async Task AddRange_RequiredPathField_ThrowsException()
    {
        // Arrange
        IEnumerable<Category> category =
        [
            Fixture
            .Build<Category>()
            .With(p => p.Path, () => null!)
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create()
        ];

        // Act
        async Task Actual()
        {
            _categoryRepository.AddRange(category);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact]
    public async Task AddRange_AddEntities_EntitiesExistInDatabase()
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

        // Act
        _categoryRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Categories.Should().BeEquivalentTo(expected);
    }
}
