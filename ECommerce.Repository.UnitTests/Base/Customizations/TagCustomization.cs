using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class TagCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Tag>(composer => composer.Without(p => p.Products).Without(p => p.Blogs));
    }
}