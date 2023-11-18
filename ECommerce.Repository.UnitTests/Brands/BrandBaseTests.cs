using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;

namespace ECommerce.Repository.UnitTests.Brands;

public class BrandBaseTests : BaseTests
{
    public readonly IBrandRepository BrandRepository;
    public BrandBaseTests()
    {
        BrandRepository = new BrandRepository(DbContext);
    }
}
