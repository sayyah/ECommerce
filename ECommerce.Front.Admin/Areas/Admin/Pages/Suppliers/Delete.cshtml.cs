using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Suppliers;

public class DeleteModel : PageModel
{
    private readonly ISupplierService _supplierService;

    public DeleteModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public Supplier Supplier { get; set; }
    [TempData] public string Message { get; set; }

    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await _supplierService.GetById(id);
        if (result.Code == 0)
        {
            Supplier = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Suppliers/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _supplierService.Delete(id);
            return RedirectToPage("/Suppliers/Index",
                new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        }

        return Page();
    }
}