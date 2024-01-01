using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class ColorCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Color>(composer => composer.Without(p => p.Prices));
    }
}