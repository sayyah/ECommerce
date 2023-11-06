using ECommerce.Domain.Entities;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[CollectionDefinition("CategoryTests", DisableParallelization = true)]
public class CategoryTestsUtils
{
    public static Dictionary<string, Dictionary<string, Category>> GetTestSets()
    {
        return new()
        {
            {
                "required",
                new()
                {
                    {
                        "required_name",
                        new() { Id = 10, Path = "1/2/3/4/5" }
                    },
                    {
                        "required_path",
                        new() { Id = 20, Name = "category 1" }
                    }
                }
            },
            {
                "simple_tests",
                new()
                {
                    {
                        "test_1",
                        new()
                        {
                            Id = 1,
                            Name = "ct1",
                            Path = "1"
                        }
                    },
                    {
                        "test_2",
                        new()
                        {
                            Id = 2,
                            Name = "ct2",
                            Path = "2",
                            IsActive = false
                        }
                    },
                    {
                        "test_3",
                        new()
                        {
                            Id = 3,
                            Name = "ct3",
                            Path = "1/3",
                            Depth = 1,
                            ParentId = 1
                        }
                    },
                    {
                        "test_4",
                        new()
                        {
                            Id = 4,
                            Name = "ct4",
                            Path = "1/3/4",
                            Depth = 2,
                            ParentId = 3
                        }
                    },
                }
            },
        };
    }
}
