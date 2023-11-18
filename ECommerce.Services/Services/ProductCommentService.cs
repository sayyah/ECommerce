namespace ECommerce.Services.Services;

public class ProductCommentService(IHttpService http) : EntityService<ProductComment, ProductComment, ProductComment>(http), IProductCommentService
{
    private const string Url = "api/ProductComments";

    public async Task<ServiceResult> Add(ProductComment productComment)
    {
        productComment.IsAccepted = false;
        productComment.IsRead = false;
        productComment.IsAnswered = false;
        productComment.DateTime = DateTime.Now;
        var result = await CreateWithoutToken(Url, productComment);
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //return Return(result);
        var result = await http.DeleteAsync(Url, id);
        if (result.Code == ResultCode.Success)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت حذف شد"
            };
        return new ServiceResult
        { Code = ServiceCode.Error, Message = "به علت وابستگی با عناصر دیگر امکان حذف وجود ندارد" };
    }

    public async Task<ServiceResult> Edit(ProductComment productComment)
    {
        var result = await Update(Url, productComment);
        //_productAttributes = null;
        return Return(result);
    }

    public async Task<ServiceResult> Accept(ProductComment productComment)
    {
        var result = await Update(Url, productComment);
        if (result.Code == ResultCode.Success && productComment.IsAccepted)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "با موفقیت تایید شد"
            };
        if (result.Code == ResultCode.Success && productComment.IsAccepted == false)
            return new ServiceResult
            {
                Code = ServiceCode.Success,
                Message = "وضعیت پیام به عدم تایید تغییر یافت"
            };
        return Return(result);
    }

    public async Task<ServiceResult<List<ProductComment>>> GetAllAcceptedComments(string search = "",
        int pageNumber = 0, int pageSize = 10)
    {
        var result = await ReadList(Url,
            $"GetAllAcceptedComments?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<ProductComment>> GetById(int id)
    {
        var result = await http.GetAsync<ProductComment>(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<List<ProductComment>>> Load(string search = "", int pageNumber = 0,
        int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }
}

