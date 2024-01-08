using MediatR;

namespace ECommerce.API.Commands
{
    public class PostBrandRequest : IRequest<Brand>
    {
        public Brand _brand { get; }

        public PostBrandRequest(Brand brand)
        {
            _brand = brand;
        }
    }
}
