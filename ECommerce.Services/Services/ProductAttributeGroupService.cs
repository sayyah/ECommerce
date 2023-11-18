namespace ECommerce.Services.Services;

public class ProductAttributeGroupService(IHttpService http) : EntityService<ProductAttributeGroup, ProductAttributeGroup, ProductAttributeGroup>(http),
    IProductAttributeGroupService
{
    private const string Url = "api/ProductAttributeGroups";
    private List<ProductAttributeGroup> _productAttributeGroups;


    public async Task<ServiceResult<List<ProductAttributeGroup>>> GetAll()
    {
        var result = await ReadList(Url, "GetAll");
        return Return(result);
    }

    public async Task<ServiceResult<List<ProductAttributeGroup>>> Load(string search = "", int pageNumber = 0,
        int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}&Search={search}");
        return Return(result);
    }

    public async Task<ServiceResult<List<ProductAttributeGroup>>> GetByProductId(int productId)
    {
        var result = await ReadList(Url, $"GetByProductId?productId={productId}");
        return Return(result);
    }

    public async Task<ServiceResult> Add(ProductAttributeGroup productAttributeGroup)
    {
        var result = await Create(Url, productAttributeGroup);
        _productAttributeGroups = null;
        return Return(result);
    }

    public async Task<ServiceResult> AddWithAttributeValue(List<ProductAttributeGroup> attributeGroups, int productId)
    {
        var result = await http.PostAsync(Url, attributeGroups, $"AddWithAttributeValue?ProductId={productId}");
        _productAttributeGroups = null;
        return Return(result);
    }

    public async Task<ServiceResult> Edit(ProductAttributeGroup productAttributeGroup)
    {
        var result = await Update(Url, productAttributeGroup);
        _productAttributeGroups = null;
        return Return(result);
    }

    public async Task<ServiceResult> Delete(int id)
    {
        //var result = await Delete(Url, id);
        //_productAttributeGroups = null;
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

    public async Task<ServiceResult<ProductAttributeGroup>> GetById(int id)
    {
        var result = await http.GetAsync<ProductAttributeGroup>(Url, $"GetById?id={id}");
        return Return(result);
    }

    public async Task<ServiceResult<List<ProductAttributeGroup>>> Load(int pageNumber = 0, int pageSize = 10)
    {
        var result = await ReadList(Url, $"Get?PageNumber={pageNumber}&PageSize={pageSize}");
        return Return(result);
    }
}