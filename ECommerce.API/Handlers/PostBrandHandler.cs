using ECommerce.API.Commands;
using ECommerce.API.Controllers;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.API.Handlers
{
    public class PostbrandHandler : ControllerBase, IRequestHandler<PostBrandRequest, Brand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<BrandsController> _logger;

        public PostbrandHandler(IBrandRepository brandRepository, ILogger<BrandsController> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<Brand?> Handle(PostBrandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return null;

            request._brand.Name = request._brand.Name.Trim();
            var repetitiveBrand = await _brandRepository.GetByName(request._brand.Name, cancellationToken);
            if (repetitiveBrand != null)
                return null;

            return await _brandRepository.AddAsync(request._brand, cancellationToken);
        }
    }
}
