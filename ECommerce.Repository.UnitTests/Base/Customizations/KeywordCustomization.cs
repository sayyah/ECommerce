using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class KeywordCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Keyword>(
            composer => composer.Without(p => p.Products).Without(p => p.Blogs)
        );
    }
}