using ECommerce.API.Commands;
using ECommerce.API.Controllers;
using ECommerce.Domain.Entities;
using MediatR;
using Sprache;

namespace ECommerce.API.Handlers
{
    public class PutBrandHandler : IRequestHandler<PutBrandRequest, Brand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<BrandsController> _logger;

        public PutBrandHandler(IBrandRepository brandRepository, ILogger<BrandsController> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }

        public async Task<Brand?> Handle(PutBrandRequest request, CancellationToken cancellationToken)
        {
            var repetitive = await _brandRepository.GetByName(request._brand.Name, cancellationToken);
            if (repetitive != null && repetitive.Id != request._brand.Id)
                return null;
            if (repetitive != null) _brandRepository.Detach(repetitive);
            return await _brandRepository.UpdateAsync(request._brand, cancellationToken);
        }
    }
}
