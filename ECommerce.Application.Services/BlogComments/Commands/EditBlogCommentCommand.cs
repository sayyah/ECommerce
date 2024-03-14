using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.BlogComments.Commands
{
    public class EditBlogCommentCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public bool IsAccepted { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
        public bool IsAnswered { get; set; }
        public string? Email { get; set; } = String.Empty;
        public string? Name { get; set; } = String.Empty;
        //ForeignKey
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? AnswerId { get; set; }
        public BlogComment? Answer { get; set; }
        public int? BlogId { get; set; }
        public Blog? Blog { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
