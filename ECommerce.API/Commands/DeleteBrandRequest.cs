using MediatR;

namespace ECommerce.API.Commands
{
    public class DeleteBrandRequest : IRequest<OkResult>
    {
        public int BrandId { get; }

        public DeleteBrandRequest(int brandID)
        {
            BrandId = brandID;
        }
    }
}
