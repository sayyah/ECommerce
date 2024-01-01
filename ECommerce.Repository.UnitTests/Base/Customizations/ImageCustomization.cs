using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class ImageCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Image>(
            composer =>
                composer
                    .Without(p => p.Product)
                    .Without(p => p.ProductId)
                    .Without(p => p.Blog)
                    .Without(p => p.BlogId)
        );
    }
}