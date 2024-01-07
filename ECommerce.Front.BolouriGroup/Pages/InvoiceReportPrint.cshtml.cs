using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Pages;

public class InvoiceReportPrintModel(IPurchaseOrderService purchaseOrderService) : PageModel
{
    public string SystemTraceNo { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; }

    public async Task OnGet(long orderId)
    {
        //SystemTraceNo = systemTraceNo;
        var result = await purchaseOrderService.GetByUserAndOrderId(orderId);
        PurchaseOrder = result.ReturnData;
    }
}