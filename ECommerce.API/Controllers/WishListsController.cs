using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Client,Admin,SuperAdmin")]
public class WishListsController(IUnitOfWork unitOfWork, ILogger<WishListsController> logger)
    : ControllerBase
{
    private readonly IHolooArticleRepository _articleRepository = unitOfWork.GetHolooRepository<IHolooArticleRepository, HolooArticle>();
    private readonly IWishListRepository _wishListRepository = unitOfWork.GetRepository<IWishListRepository, WishList>();

    [HttpGet]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _wishListRepository.GetByIdWithInclude(id, cancellationToken);
            if (result != null)
            {
                var prices = result.Select(x => x.Price).ToList();
                var aCodeCs = prices.Select(x => x.ArticleCodeCustomer).ToList();
                var holooArticle = await _articleRepository.GetHolooArticles(aCodeCs!, cancellationToken);
                foreach (var wishListViewModel in result)
                {
                    await _articleRepository.AddPrice(
                       new List<Price> { wishListViewModel.Price },
                       holooArticle,
                       false,
                       cancellationToken);
                }
                await unitOfWork.SaveAsync(cancellationToken);
            }

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(WishList? wishList, CancellationToken cancellationToken)
    {
        try
        {
            if (wishList == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var repetitiveWishList =
                await _wishListRepository.GetByProductUser(wishList.PriceId, wishList.UserId, cancellationToken);
            if (repetitiveWishList != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { repetitiveWishList.Id.ToString() }
                });

            _wishListRepository.Add(wishList);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _wishListRepository.DeleteById(id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Invert(WishList wishList, CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _wishListRepository.Where(x => x.UserId == wishList.UserId && x.PriceId == wishList.PriceId,
                    cancellationToken);
            if (result != null)
            {
                var wishLists = result.ToList();
                if (!wishLists.Any())
                {
                    _wishListRepository.Add(wishList);
                    await unitOfWork.SaveAsync(cancellationToken);
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Success,
                        Messages = new List<string> { "به لیست علاقه مندی ها اضافه شد" }
                    });
                }
                await _wishListRepository.DeleteById(wishLists.FirstOrDefault()!.Id, cancellationToken);
                await unitOfWork.SaveAsync(cancellationToken);
            }

            

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                Messages = new List<string> { "از لیست علاقه مندی ها حذف شد" }
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}