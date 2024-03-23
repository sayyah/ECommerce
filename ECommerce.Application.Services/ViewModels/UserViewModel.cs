using ECommerce.Application.Services.Objects;
using ECommerce.Domain.Entities.Helper;

namespace ECommerce.Application.ViewModels;

public class UserViewModel
{
}

public class UserListViewModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? UserRole { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public decimal BuyingAmount { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsActive { get; set; } = false;
    public bool IsColleague { get; set; } = false;
}

public class userFilterParameters
{
    public PaginationParameters? PaginationParameters { get; set; }
    public UserSort UserSort { get; set; }
    public bool? IsColleauge { get; set; }
    public bool? HasBuying { get; set; }
    public bool? IsActive { get; set; }
}
