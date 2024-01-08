using ECommerce.API.Commands;
using ECommerce.API.Controllers;
using MediatR;

namespace ECommerce.API.Handlers
{
    public class DeleteBrandhandler : ControllerBase, IRequestHandler<DeleteBrandRequest, OkResult>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger<BrandsController> _logger;

        public DeleteBrandhandler(IBrandRepository brandRepository, ILogger<BrandsController> logger)
        {
            _brandRepository = brandRepository;
            _logger = logger;
        }
        public async Task<OkResult> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
        {

            await _brandRepository.DeleteAsync(request.BrandId, cancellationToken);
            return Ok();
        }
    }
}
