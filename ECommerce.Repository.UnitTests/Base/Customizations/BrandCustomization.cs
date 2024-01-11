using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class BrandCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Brand>(
            composer => composer.Without(p => p.Products).Without(p => p.Image)
        );
    }
}