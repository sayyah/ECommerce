using AutoFixture;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async Task Delete_NullBlogCategory_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCategoryRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task Delete_DeleteBlogCategory_EntityNotInRepositoryAsync()
    {
        // Arrange
        BlogCategory root = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .With(p => p.Blogs, () => [ ])
            .Create();
        BlogCategory child1 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        BlogCategory child2 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        root.BlogCategories!.Add(child1);
        root.BlogCategories.Add(child2);
        List<BlogCategory> expected =  [ root, child1, child2 ];
        DbContext.BlogCategories.AddRange(expected);
        DbContext.SaveChanges();

        BlogCategory blogCategoryToDelete = root;

        // Act
        _blogCategoryRepository.Delete(blogCategoryToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogCategory? actual = DbContext
            .BlogCategories
            .FirstOrDefault(x => x.Id == blogCategoryToDelete.Id);

        // Assert
        Assert.Null(actual);
        Assert.Equal(expected.Count - 1, DbContext.BlogCategories.Count());
    }
}
