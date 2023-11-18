using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void UpdateRange_NullBlogCategory_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.UpdateRange([null!]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task UpdateRange_UpdateEntities_EntitiesChangeAsync()
    {
        // Arrange
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
        DbContext.BlogCategories.AddRange(expected);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (var entry in expected)
        {
            entry.Name = Guid.NewGuid().ToString();
            entry.Description = Guid.NewGuid().ToString();
        }

        // Act
        _blogCategoryRepository.UpdateRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        List<BlogCategory?> actual = [.. DbContext.BlogCategories];

        // Assert
        actual.Should().BeEquivalentTo(expected).And.HaveCount(3);
    }
}
