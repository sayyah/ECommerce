using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Domain.Interfaces;

public interface IHolooArticleRepository : IHolooRepository<HolooArticle>
{
    Task<List<HolooArticle>> GetAllArticleMCodeSCode(string code, CancellationToken cancellationToken,
        short sendToSite = 0);

    Task<List<HolooArticle>>
        GetAllMCode(string mCode, CancellationToken cancellationToken, short sendToSite = 0);

    void SyncHolooWebId(string aCodeC, int productId);

    Task<List<HolooArticle>> GetHolooArticles(List<string> aCodeCs, CancellationToken cancellationToken);

    Task<(decimal price, double? exist, List<string> a_Code)> GetHolooPrice(string aCodeC, Price.HolooSellNumber sellPrice);

    Task<List<T>> AddPriceAndExistFromHolooList<T>(
        IList<T> products, bool isWithoutBill, bool? isCheckExist, CancellationToken cancellationToken)
        where T : BaseProductPageViewModel;

    Task<List<Price>> AddPrice(List<Price> prices, IEnumerable<HolooArticle> holooArticles, bool? isCheckExist,
        CancellationToken cancellationToken);

    Task<List<HolooArticle>> GetHolooArticlesDefaultWarehouse(List<string?> aCodeCs,
        CancellationToken cancellationToken);

    Task<List<HolooArticle>> GetHolooArticlesOtherWarehouse(List<string?> aCodeCs,
        CancellationToken cancellationToken);
}
