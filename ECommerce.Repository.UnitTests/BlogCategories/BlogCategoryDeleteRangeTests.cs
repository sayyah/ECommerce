using AutoFixture;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async Task DeleteRange_NullBlogCategory_ThrowsException()
    {
        // Act
        async Task Actual()
        {
            _blogCategoryRepository.DeleteRange([ null! ]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Actual);
    }

    [Fact]
    public async Task DeleteRange_NullArgument_ThrowsExceptionAsync()
    {
        // Act
        async Task ActualAsync()
        {
            _blogCategoryRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(ActualAsync);
    }

    [Fact]
    public async Task DeleteRange_DeleteBlogCategories_EntityNotInRepositoryAsync()
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

        BlogCategory blogCategoryNotToDelete = root;
        IEnumerable<BlogCategory> blogCategorysToDelete = expected.Where(
            x => x.Id != blogCategoryNotToDelete.Id
        );

        // Act
        _blogCategoryRepository.DeleteRange(blogCategorysToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Equal(1, DbContext.BlogCategories.Count());
        Assert.Equivalent(blogCategoryNotToDelete, DbContext.BlogCategories.FirstOrDefault());
    }
}
