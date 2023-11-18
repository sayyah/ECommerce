using AutoFixture;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public async Task Update_UpdateEntity_EntityChangesAsync()
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

        BlogCategory expectedBlogCategory = child1;
        expectedBlogCategory.Name = Guid.NewGuid().ToString();
        expectedBlogCategory.Description = Guid.NewGuid().ToString();

        // Act
        _blogCategoryRepository.Update(expectedBlogCategory);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogCategory? actual = DbContext
            .BlogCategories
            .Single(p => p.Id == expectedBlogCategory.Id);

        // Assert
        Assert.Equivalent(expectedBlogCategory, actual);
    }
}
