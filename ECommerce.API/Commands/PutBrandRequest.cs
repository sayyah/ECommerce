using MediatR;

namespace ECommerce.API.Commands
{
    public class PutBrandRequest : IRequest<Brand>
    {
        public Brand _brand { get; }

        public PutBrandRequest(Brand brand)
        {
            _brand = brand;
        }
    }
}
