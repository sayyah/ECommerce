using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class DiscountCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Discount>(
            composer =>
                composer
                    .Without(p => p.Prices)
                    .Without(p => p.Categories)
                    .Without(p => p.PurchaseOrders)
                    .Without(p => p.PurchaseOrderDetails)
        );
    }
}