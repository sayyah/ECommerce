using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Suppliers;

public class DetailModel(ISupplierService supplierService) : PageModel
{
    public Supplier Supplier { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await supplierService.GetById(id);
        if (result.Code == 0)
        {
            Supplier = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Suppliers/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }
}