using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class InvoiceReportPrintModel : PageModel
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public InvoiceReportPrintModel(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    public string SystemTraceNo { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; }

    public async Task OnGet(long orderId)
    {
        //SystemTraceNo = systemTraceNo;
        var result = await _purchaseOrderService.GetByUserAndOrderId(orderId);
        PurchaseOrder = result.ReturnData;
    }
}