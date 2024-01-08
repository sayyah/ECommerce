using MediatR;

namespace ECommerce.API.Queries
{
    public class GetByIDBrandQuery : IRequest<Brand>
    {
        public int BrandId { get; }

        public GetByIDBrandQuery(int brandID)
        {
            BrandId = brandID;
        }
    }
}
