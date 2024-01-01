using AutoFixture;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

[Collection("BlogTests")]
public partial class BlogTests : BaseTests
{
    private readonly IBlogRepository _blogRepository;

    public BlogTests()
    {
        _blogRepository = new BlogRepository(DbContext);
        Fixture.Customize<Blog>(composer =>
        {
            var blogCategory = Fixture
                .Build<BlogCategory>()
                .With(p => p.BlogCategories, () => null)
                .With(p => p.Parent, () => null)
                .With(p => p.ParentId, () => null)
                .With(p => p.Blogs, () => [ ])
                .Create();
            var blogAuthor = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).Create();
            var image = Fixture
                .Build<Image>()
                .Without(p => p.Blog)
                .Without(p => p.BlogId)
                .Without(p => p.Product)
                .Without(p => p.ProductId)
                .Create();
            var tags = Fixture
                .Build<Tag>()
                .Without(p => p.Blogs)
                .Without(p => p.Products)
                .CreateMany(2);
            var keywords = Fixture
                .Build<Keyword>()
                .Without(p => p.Blogs)
                .Without(p => p.Products)
                .CreateMany(2);

            return composer
                .With(p => p.BlogAuthor, blogAuthor)
                .With(p => p.BlogAuthorId, blogAuthor.Id)
                .With(p => p.BlogCategory, blogCategory)
                .With(p => p.BlogCategoryId, blogCategory.Id)
                .With(p => p.Keywords, keywords.ToList())
                .With(p => p.Tags, tags.ToList())
                .With(p => p.Image, image)
                .Without(p => p.BlogComments);
        });
    }
}
