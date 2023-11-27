using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[CollectionDefinition("BlogCategories", DisableParallelization = true)]
public class BlogCategoryTestUtils
{
    public static Dictionary<string, Dictionary<string, BlogCategory>> TestSets =>
        new()
        {
            ["required_fields"] = new()
            {
                ["no_name"] = new() { Id = 1, Description = "test description" },
            },
            ["simple_tests"] = new()
            {
                ["test_1"] = new()
                {
                    Id = 3,
                    Name = "root1",
                    Description = "test description"
                },
                ["test_2"] = new()
                {
                    Id = 4,
                    Name = "root2",
                    Description = "description 2"
                },
                ["test_3"] = new()
                {
                    Id = 5,
                    Name = "child of root 1",
                    Description = "description the third!",
                    ParentId = 3,
                    Depth = 1
                },
                ["test_4"] = new()
                {
                    Id = 6,
                    Name = "child of root 2",
                    Description = Guid.NewGuid().ToString(),
                    ParentId = 4,
                    Depth = 1
                },
                ["test_5"] = new()
                {
                    Id = 7,
                    Name = "child of child of root 1",
                    Description = Guid.NewGuid().ToString(),
                    ParentId = 5,
                    Depth = 2
                },
            },
        };
}
