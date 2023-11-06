using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.PersianTranslations.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PersianRequiredAttribute : RequiredAttribute
{
    public override string FormatErrorMessage(string name)
    {
        return $"وارد {name} الزامی است";
    }
}
