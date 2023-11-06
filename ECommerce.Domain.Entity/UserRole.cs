using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public class UserRole : IdentityRole<int>, IBaseEntity<int>
{
}