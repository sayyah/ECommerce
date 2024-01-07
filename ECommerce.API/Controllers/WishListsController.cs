namespace ECommerce.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Client,Admin,SuperAdmin")]
public class WishListsController(IWishListRepository wishListRepository,
        ILogger<WishListsController> logger,
        IHolooArticleRepository articleRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await wishListRepository.GetByIdWithInclude(id, cancellationToken);
            var prices = result.Select(x => x.Price).ToList();
            var aCodeCs = prices.Select(x => x.ArticleCodeCustomer).ToList();
            var holooArticle = await articleRepository.GetHolooArticles(aCodeCs, cancellationToken);
            foreach (var wishListViewModel in result)
            {
                var holooPrices = await articleRepository.AddPrice(
                    new List<Price> { wishListViewModel.Price },
                    holooArticle,
                    false,
                    cancellationToken);
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
    public async Task<IActionResult> Post(WishList wishList, CancellationToken cancellationToken)
    {
        try
        {
            if (wishList == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var repetitiveWishList =
                await wishListRepository.GetByProductUser(wishList.PriceId, wishList.UserId, cancellationToken);
            if (repetitiveWishList != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Repetitive,
                    Messages = new List<string> { repetitiveWishList.Id.ToString() }
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = await wishListRepository.AddAsync(wishList, cancellationToken)
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
            await wishListRepository.DeleteAsync(id, cancellationToken);
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
                await wishListRepository.Where(x => x.UserId == wishList.UserId && x.PriceId == wishList.PriceId,
                    cancellationToken);
            if (result != null && result.ToList().Count == 0)
                return Ok(new ApiResult
                {
                    Code = ResultCode.Success,
                    ReturnData = await wishListRepository.AddAsync(wishList, cancellationToken),
                    Messages = new List<string> { "به لیست علاقه مندی ها اضافه شد" }
                });
            await wishListRepository.DeleteAsync(result.FirstOrDefault().Id, cancellationToken);
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