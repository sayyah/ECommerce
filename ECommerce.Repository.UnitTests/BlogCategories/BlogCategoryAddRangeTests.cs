using AutoFixture;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async Task AddRange_RequiredNameField_ThrowsException()
    {
        // Arrange
        List<BlogCategory> blogCategoryList =
        [
            Fixture
            .Build<BlogCategory>()
            .With(p => p.Name, () => null!)
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => null!)
            .With(p => p.Blogs, () => [])
            .Create()
        ];

        // Act
        async Task Actual()
        {
            _blogCategoryRepository.AddRange(blogCategoryList);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Actual);
    }

    [Fact]
    public async Task AddRange_NullBlogCategory_ThrowsException()
    {
        // Act
        async Task Actual()
        {
            _blogCategoryRepository.AddRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Actual);
    }

    [Fact]
    public async Task AddRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Actual()
        {
            _blogCategoryRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task AddRange_AddEntities_EntityExistsInRepositoryAsync()
    {
        BlogCategory root = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .With(p => p.Blogs, () => [])
            .Create();
        BlogCategory child1 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [])
            .Create();
        BlogCategory child2 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [])
            .Create();
        root.BlogCategories!.Add(child1);
        root.BlogCategories.Add(child2);
        List<BlogCategory> expected = [root, child1, child2];

        // Act
        _blogCategoryRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        List<BlogCategory> actual = [.. DbContext.BlogCategories];
        actual.Should().BeEquivalentTo(expected);
    }
}
