using Azure;
using ECommerce.Domain.Entities;
using ECommerce.Handler.UnitTests.Base;

namespace ECommerce.Handler.UnitTests.Blogs
{
    public class CreateBlogCommandBaseTests : BaseTests
    {
        public Tag AddTags(int id)
        {
            Tag tag = new()
            {
                Id = id,
                TagText = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            DbContext.Tags.Add(tag);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();
            return tag;
        }

        public Keyword AddKeyword(int id)
        {
            Keyword keyword = new()
            {
                Id = id,
                KeywordText = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            DbContext.Keywords.Add(keyword);
            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();
            return keyword;
        }
    }
}
