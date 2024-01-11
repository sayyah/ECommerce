using ECommerce.Domain.Entities.HolooEntity;

namespace ECommerce.Application.ViewModels;

public class FactorViewModel
{
    public HolooFBail HolooFBail { get; set; }
    public List<HolooABail> HolooABails { get; set; }
}