using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async Task AddAsync_NullArgument_ThrowsException()
    {
        // Act
        async Task<Category> Action()
        {
            var categories = await _categoryRepository.AddAsync(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            return categories;
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task AddAsync_RequiredNameField_ThrowsException()
    {
        // Arrange
        Category category = Fixture
            .Build<Category>()
            .With(p => p.Name, () => null!)
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();

        // Act
        async Task<Category> Action()
        {
            var res = await _categoryRepository.AddAsync(category, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            return res;
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task AddAsync_RequiredPathField_ThrowsException()
    {
        // Arrange
        Category category = Fixture
            .Build<Category>()
            .With(p => p.Path, () => null!)
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();

        // Act
        async Task<Category> Action()
        {
            var res = await _categoryRepository.AddAsync(category, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
            return res;
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        Category expected = Fixture
            .Build<Category>()
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .Create();

        // Act
        await _categoryRepository.AddAsync(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Categories.FirstOrDefault();

        // Assert
        DbContext.Categories.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
