using ECommerce.API.Controllers;
using ECommerce.API.Queries;
using MediatR;

namespace ECommerce.API.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, List<Brand>>
    {

        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<BrandsController> _logger;

        public GetAllBrandsHandler(IBrandRepository brandRepository, ILogger<BrandsController> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<List<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var result = await _brandRepository.GetAll(cancellationToken);
            return result.ToList();
        }
    }
}
