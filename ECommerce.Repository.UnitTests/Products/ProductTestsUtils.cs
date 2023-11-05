using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[CollectionDefinition("ProductTests", DisableParallelization = true)]
public class ProductTestUtils
{
    public static List<Price> GetSamplePrices()
    {
        return [
            new Price()
            {
                Id = 2,
                ProductId = 4,
                Amount = 200,
                Exist = 20,
                MaxQuantity = 20,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 3,
                ProductId = 5,
                Amount = 1000,
                Exist = 5,
                MaxQuantity = 5,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 4,
                ProductId = 398,
                Amount = 10010,
                Exist = 20,
                MaxQuantity = 20,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 5,
                ProductId = -92764,
                Amount = 2222,
                Exist = 5,
                MaxQuantity = 5,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 6,
                ProductId = 420,
                Amount = 92784,
                Exist = 5,
                MaxQuantity = 5,
                MinQuantity = 1,
            },
        ];
    }

    public static List<Category> GetSampleCategories()
    {
        return [
            new Category()
            {
                Id = 10,
                Name = "test category 1",
                Depth = 0,
                Path = "test1",
                IsActive = true,
                ParentId = null,
            },
            new Category()
            {
                Id = 20,
                Name = "test category 2",
                Depth = 1,
                Path = "test1/test2",
                IsActive = true,
                ParentId = 10,
            },
            new Category()
            {
                Id = 30,
                Name = "test category 3",
                Depth = 1,
                Path = "test1/test3",
                IsActive = true,
                ParentId = 10,
            },
            new Category()
            {
                Id = 40,
                Name = "test category 4",
                Depth = 1,
                Path = "test1/test4",
                IsActive = false,
                ParentId = 10,
            },
        ];
    }

    public static Dictionary<string, Dictionary<string, Product>> GetTestSets()
    {
        List<Price> prices = GetSamplePrices();
        List<Category> categories = GetSampleCategories();

        return new()
        {
            {
                "null_test",
                new() { { "null_object", null! } }
            },
            {
                "required_fields",
                new()
                {
                    {
                        "required_Name",
                        new Product
                        {
                            Id = 1,
                            MinOrder = 10,
                            Url = "some random-url/w.[]"
                        }
                    },
                    {
                        "required_MinOrder",
                        new Product
                        {
                            Id = 2,
                            Name = "this has name",
                            Url = "some random-url/w.[/\\:D]"
                        }
                    },
                    {
                        "required_Url",
                        new Product
                        {
                            Id = 3,
                            Name = "this also has name",
                            MinOrder = 2
                        }
                    },
                }
            },
            {
                "duplicate_url",
                new()
                {
                    {
                        "test_1",
                        new Product
                        {
                            Id = 4,
                            Name = "this has name",
                            IsShowInIndexPage = true,
                            Description = new string('*', 500),
                            Review = "I'll let this one be short :>",
                            Star = -2, // should it accept this?!
                            MinOrder = 10,
                            MaxOrder = 255,
                            MinInStore = -10, // should it accept this?!
                            ReorderingLevel = 20,
                            IsDiscontinued = true,
                            IsActive = true,
                            Url = "some random-url/w.[/\\:D]",
                            Prices =  [ prices[0] ],
                            ProductCategories = new List<Category> { categories[1] }
                        }
                    },
                    {
                        "test_2",
                        new Product
                        {
                            Id = 5,
                            Name = "this name",
                            IsShowInIndexPage = true,
                            Description = "This time I'll let this one be short :>",
                            Review = new string('*', 500),
                            Star = 2,
                            MinOrder = 10,
                            MaxOrder = 255,
                            MinInStore = 3,
                            ReorderingLevel = 20,
                            IsDiscontinued = true,
                            IsActive = true,
                            Url = "some random-url/w.[/\\:D]", // should it accept the same url as test_1?!
                            Prices =  [ prices[1] ],
                            ProductCategories = new List<Category> { categories[2] }
                        }
                    },
                }
            },
            {
                "unique_url",
                new()
                {
                    {
                        "test_1",
                        new Product
                        {
                            Id = 398,
                            Name = "this has name",
                            MinOrder = 10,
                            Url = "some random-url/w.[/\\:D]",
                            Prices =  [ prices[2] ],
                            ProductCategories = new List<Category> { categories[1] }
                        }
                    },
                    {
                        "test_2",
                        new Product
                        {
                            Id = -92764,
                            Name = "this name",
                            MinOrder = 10,
                            Url = "random-url",
                            Prices =  [ prices[3] ],
                            ProductCategories = new List<Category> { categories[2] }
                        }
                    },
                    {
                        "test_3",
                        new Product
                        {
                            Id = 420,
                            Name = "such product",
                            MinOrder = 69,
                            Url = "much/wow",
                            Prices =  [ prices[4] ],
                            ProductCategories = new List<Category> { categories[3] }
                        }
                    },
                }
            }
        };
    }
}
