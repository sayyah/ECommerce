using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

public partial class CategoryTests
{
    [Fact]
    public async void Add_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _categoryRepository.Add(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Add_RequiredNameField_ThrowsException()
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
        async Task Action()
        {
            _categoryRepository.Add(category);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_RequiredPathField_ThrowsException()
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
        async Task Action()
        {
            _categoryRepository.Add(category);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_AddEntities_EntitiesExistInDatabase()
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
            .Create();

        // Act
        _categoryRepository.Add(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        Category actual = DbContext.Categories.Single(c => c.Id == expected.Id);

        // Assert
        DbContext.Categories.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
