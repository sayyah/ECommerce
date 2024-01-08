using MediatR;

namespace ECommerce.API.Queries
{
    public class GetAllBrandsQuery : IRequest<List<Brand>>
    {
    }
}
