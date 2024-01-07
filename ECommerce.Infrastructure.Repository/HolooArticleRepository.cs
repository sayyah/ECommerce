using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Infrastructure.Repository;

public class HolooArticleRepository(HolooDbContext context,
        IHolooABailRepository aBailRepository)
    : HolooRepository<HolooArticle>(context), IHolooArticleRepository
{
    public async Task SyncHolooWebId(string aCodeC, int productId, CancellationToken cancellationToken)
    {
        var articles = context.ARTICLE.Where(x => x.A_Code_C == aCodeC);
        foreach (var article in articles)
        {
            article.WebId = productId;
            context.ARTICLE.Update(article);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(decimal price, double? exist, List<string> a_Code)> GetHolooPrice(string aCodeC, Price.HolooSellNumber sellPrice)
    {
        var bolooryFlag = (await context.Customer.FirstAsync()).C_Name == "گروه تجهيزات صنعتي بلوري";
        var article = await context.ARTICLE
            .Where(x => x.A_Code_C.Equals(aCodeC) && (Convert.ToInt32(x.A_Code) < 3400000 || !bolooryFlag))
            .ToListAsync();
        decimal price = 0;
        switch (sellPrice)
        {
            case Price.HolooSellNumber.Sel_Price:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price);
                break;
            case Price.HolooSellNumber.Sel_Price2:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price2);
                break;
            case Price.HolooSellNumber.Sel_Price3:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price3);
                break;
            case Price.HolooSellNumber.Sel_Price4:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price4);
                break;
            case Price.HolooSellNumber.Sel_Price5:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price5);
                break;
            case Price.HolooSellNumber.Sel_Price6:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price6);
                break;
            case Price.HolooSellNumber.Sel_Price7:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price7);
                break;
            case Price.HolooSellNumber.Sel_Price8:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price8);
                break;
            case Price.HolooSellNumber.Sel_Price9:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price9);
                break;
            case Price.HolooSellNumber.Sel_Price10:
                price = Convert.ToUInt64(article.FirstOrDefault().Sel_Price10);
                break;
        }
        List<string> a_code = new List<string>();
        foreach (var item in article)
        {
            if (item.Exist > 0)
                a_code.Add(item.A_Code);
        }
        return (price, article.Sum(x => x.Exist), a_code);
    }

    public async Task<List<T>> AddPriceAndExistFromHolooList<T>(
        IList<T> products, bool isWithoutBill, bool? isCheckExist, CancellationToken cancellationToken)
        where T : BaseProductPageViewModel
    {
        isWithoutBill = true;
        var prices = products
            .Where(x => x.Prices.Any(p => p.ArticleCode != null))
            .Select(p => p.Prices)
            .ToList();
        var aCodeCs = new List<string>();
        foreach (var price in prices)
        {
            foreach (var aCode in price) aCodeCs.Add(aCode.ArticleCodeCustomer);
        }

        products = products.Where(x => x.Prices.Any(p => p.ArticleCode != null)).ToList();
        var holooArticle = await GetHolooArticles(aCodeCs, cancellationToken);

        List<T> newProducts = new();
        foreach (var product in products)
        {
            var tempPriceList = await AddPrice(product.Prices, holooArticle, isCheckExist, cancellationToken);

            product.Prices.RemoveAll(x => !tempPriceList.Contains(x));
            if (product.Prices.Count != 0) newProducts.Add(product);
        }

        ;

        return newProducts;
    }

    public async Task<IEnumerable<HolooArticle>> GetAllArticleMCodeSCode(string code,
        CancellationToken cancellationToken, short sendToSite = 0)
    {
        return await context.ARTICLE.Where(x => x.A_Code.StartsWith(code)
                                                 && x.SendToSite == sendToSite && !x.Model.Equals("#*Not_Article*#"))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HolooArticle>> GetAllMCode(string mCode, CancellationToken cancellationToken,
        short sendToSite = 0)
    {
        return await context.ARTICLE
            .Where(x => x.A_Code.StartsWith(mCode) && x.SendToSite == sendToSite && !x.Model.Equals("#*Not_Article*#"))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HolooArticle>> GetHolooArticles(List<string> aCodeCs,
        CancellationToken cancellationToken)
    {
        var temp = context.ARTICLE.Where(
            x => aCodeCs.Any(
                aCodeC => aCodeC == x.A_Code_C));
        var bolooryFlag = (await context.Customer.FirstAsync()).C_Name == "گروه تجهيزات صنعتي بلوري";
        return await temp.Where(x => Convert.ToInt32(x.A_Code) < 3400000 || !bolooryFlag)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HolooArticle>> GetHolooArticlesDefaultWarehouse(List<string> aCodeCs,
        CancellationToken cancellationToken)
    {
        var temp = context.ARTICLE.Where(
            x => aCodeCs.Any(
                aCodeC => aCodeC == x.A_Code_C));
        var bolooryFlag = (await context.Customer.FirstAsync()).C_Name == "گروه تجهيزات صنعتي بلوري";
        return await temp
            .Where(x => Convert.ToInt32(x.A_Code) < 3400000 && Convert.ToInt32(x.A_Code) > 2400000 || !bolooryFlag)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HolooArticle>> GetHolooArticlesOthereWarehouse(List<string> aCodeCs,
        CancellationToken cancellationToken)
    {
        var temp = context.ARTICLE.Where(
            x => aCodeCs.Any(
                aCodeC => aCodeC == x.A_Code_C));
        var bolooryFlag = (await context.Customer.FirstAsync()).C_Name == "گروه تجهيزات صنعتي بلوري";
        return await temp.Where(x => Convert.ToInt32(x.A_Code) < 900000 || !bolooryFlag).ToListAsync(cancellationToken);
    }

    public async Task<List<Price>> AddPrice(List<Price> prices, IEnumerable<HolooArticle> holooArticles,
        bool? isCheckExist, CancellationToken cancellationToken)
    {
        List<Price> tempPriceList = new();
        foreach (var productPrices in prices)
            if (productPrices.SellNumber != null && productPrices.SellNumber != Price.HolooSellNumber.خالی)
            {
                var article = holooArticles.Where(x => x.A_Code_C == productPrices.ArticleCodeCustomer).ToList();
                decimal articlePrice = 0;
                if (article.Count > 0)
                {
                    try
                    {
                        switch (productPrices.SellNumber)
                        {
                            case Price.HolooSellNumber.Sel_Price:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price);
                                break;
                            case Price.HolooSellNumber.Sel_Price2:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price2);
                                break;
                            case Price.HolooSellNumber.Sel_Price3:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price3);
                                break;
                            case Price.HolooSellNumber.Sel_Price4:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price4);
                                break;
                            case Price.HolooSellNumber.Sel_Price5:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price5);
                                break;
                            case Price.HolooSellNumber.Sel_Price6:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price6);
                                break;
                            case Price.HolooSellNumber.Sel_Price7:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price7);
                                break;
                            case Price.HolooSellNumber.Sel_Price8:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price8);
                                break;
                            case Price.HolooSellNumber.Sel_Price9:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price9);
                                break;
                            case Price.HolooSellNumber.Sel_Price10:
                                articlePrice = Convert.ToDecimal(article.FirstOrDefault().Sel_Price10);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        //_logger.LogCritical(e,
                        //    $"Product Prices ID : {productPrices.Id}, Article Code : {productPrices.ArticleCode}, Article Code Customer : {productPrices.ArticleCodeCustomer}");
                    }

                    if (articlePrice < 10)
                        //product.Prices.Remove(productPrices);
                        //tempPriceList.Add(productPrices);
                        continue;

                    productPrices.Amount = articlePrice / 10;
                    double soldExist = 0;
                    List<string> a_code = new List<string>();
                    foreach (var item in article)
                    {
                        
                            a_code.Add(item.A_Code);
                    }
                    if (productPrices.ArticleCode != null)
                    {
                        //int userCode = Convert.ToInt32(_configuration.GetValue<string>("UserCode"));
                        var userCode = 15;
                        foreach (var item in a_code)
                            soldExist += aBailRepository.GetWithACode(userCode, item, cancellationToken);
                    }

                    productPrices.Exist = article.Sum(x => x.Exist) - soldExist ?? 0;

                    if (isCheckExist == true && productPrices.Exist == 0)
                        //product.Prices.Remove(productPrices);
                        //tempPriceList.Add(productPrices);
                        continue;
                    tempPriceList.Add(productPrices);
                }
            }

        return tempPriceList;
    }
}
