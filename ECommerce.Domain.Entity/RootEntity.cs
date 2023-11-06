namespace ECommerce.Domain.Entities;

public abstract class RootEntity : BaseEntity
{
    public int? CreatorUserId { get; set; }
    public int? EditorUserId { get; set; }
<<<<<<< HEAD
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; } 
=======
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
}
