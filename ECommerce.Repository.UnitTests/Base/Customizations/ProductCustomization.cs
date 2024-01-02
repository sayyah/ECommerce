using AutoFixture;
using ECommerce.Domain.Entities;

namespace ECommerce.Repository.UnitTests.Base.Customizations;

public class ProductCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Product>(composer =>
        {
            var prices = fixture.CreateMany<Price>(1).ToList();
            var images = fixture.CreateMany<Image>(1).ToList();
            var brand = fixture.Create<Brand>();
            var keywords = fixture.CreateMany<Keyword>(1).ToList();
            var tags = fixture.CreateMany<Tag>(1).ToList();

            return composer
                .With(p => p.Prices, prices)
                .With(p => p.Images, images)
                .With(p => p.Brand, brand)
                .With(p => p.BrandId, brand.Id)
                .With(p => p.Keywords, keywords)
                .With(p => p.Tags, tags)
                .Without(p => p.ProductCategories)
                .Without(p => p.ProductComments)
                .Without(p => p.ProductUserRanks)
                .Without(p => p.AttributeGroupProducts)
                .Without(p => p.AttributeValues)
                .Without(p => p.Supplier)
                .Without(p => p.SupplierId)
                .Without(p => p.SlideShows)
                .Without(p => p.HolooCompanyId)
                .Without(p => p.HolooCompany)
                .Without(p => p.Store)
                .Without(p => p.StoreId);
        });
    }
}