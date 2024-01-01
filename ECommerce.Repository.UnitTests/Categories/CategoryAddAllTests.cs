using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async Task AddAll_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _categoryRepository.AddAll(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async Task AddAll_RequiredNameField_ThrowsException()
    {
        // Arrange
        IEnumerable<Category> categories =
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
                .Create(),
        ];

        // Act
        async Task Actual()
        {
            await _categoryRepository.AddAll(categories, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact]
    public async Task AddAll_RequiredPathField_ThrowsException()
    {
        // Arrange
        IEnumerable<Category> categories =
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
                .Create(),
        ];

        // Act
        async Task Actual()
        {
            await _categoryRepository.AddAll(categories, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact]
    public async void AddAll_AddEntities_EntitiesExistInDatabase()
    {
        // Arrange
        var expected = Fixture
            .Build<Category>()
            .Without(p => p.Parent)
            .Without(p => p.ParentId)
            .Without(p => p.Categories)
            .Without(p => p.Products)
            .Without(p => p.SlideShows)
            .Without(p => p.Discount)
            .Without(p => p.DiscountId)
            .CreateMany();

        // Act
        await _categoryRepository.AddAll(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Categories.ToList();

        // Assert
        Assert.Equal(expected.Count(), actual.Count);
        DbContext.Categories.ToArray().Should().BeEquivalentTo(expected);
    }
}
