using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class PriceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Price>(composer =>
        {
            var discount = fixture.Create<Discount>();
            var color = fixture.Create<Color>();

            return composer
                .With(p => p.Discount, discount)
                .With(p => p.DiscountId, discount.Id)
                .With(p => p.Color, color)
                .With(p => p.ColorId, color.Id)
                .Without(p => p.ProductId)
                .Without(p => p.Product)
                .Without(p => p.CurrencyId)
                .Without(p => p.Currency)
                .Without(p => p.SizeId)
                .Without(p => p.Size)
                .Without(p => p.UnitId)
                .Without(p => p.Unit);
        });
    }
}