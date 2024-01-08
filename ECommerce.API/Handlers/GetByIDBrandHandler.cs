using ECommerce.API.Controllers;
using ECommerce.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ECommerce.API.Handlers
{
    public class GetByIDBrandHandler : IRequestHandler<GetByIDBrandQuery, Brand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<BrandsController> _logger;

        public GetByIDBrandHandler(IBrandRepository brandRepository, ILogger<BrandsController> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<Brand?> Handle(GetByIDBrandQuery request, CancellationToken cancellationToken)
        {
            var result = await _brandRepository.GetByIdAsync(cancellationToken, request.BrandId);

            if (result == null)
                return null;
            return result;
        }
    }
}
