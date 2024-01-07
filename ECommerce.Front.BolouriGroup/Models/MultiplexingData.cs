using System.ComponentModel.DataAnnotations;

namespace ECommerce.Front.BolouriGroup.Models;

public class MultiplexingData
{
    public enum MultiplexingType
    {
        Percentage,
        Amount
    }

    [Display(Name = @"نوع تسهیم")] public MultiplexingType? Type { get; set; }

    public List<MultiplexingDataItem> MultiplexingRows { get; set; } = new();

    public bool IsValid()
    {
        if (!Type.HasValue) return false;
        if (!MultiplexingRows.Any()) return false;
        if (MultiplexingRows.Any(t => t.Value < 0)) return false;

        switch (Type.Value)
        {
            case MultiplexingType.Percentage:
                if (MultiplexingRows.Sum(t => t.Value) > 100)
                    return false;
                if (MultiplexingRows.Any(t => t.Value > 99))
                    return false;
                break;
            case MultiplexingType.Amount:
                break;
        }

        return true;
    }

    public class MultiplexingDataItem
    {
        public int IbanNumber { get; set; }
        public long Value { get; set; }
    }
}