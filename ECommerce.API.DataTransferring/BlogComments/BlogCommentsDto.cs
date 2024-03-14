using ECommerce.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DataTransferObject.BlogComments;
public class BlogCommentDto : BaseDto, IBlogCommentDto
{
    public string Text { get; set; }
    public bool IsAccepted { get; set; }
    public string? Name { get; set; }
    public string? BlogTitle { get; set; }
    public bool IsRead { get; set; }
    public bool IsAnswered { get; set; }
    public DateTime DateTime { get; set; }= DateTime.Now;
    public int? AnswerId { get; set; }
    public string? AnswerText { get; set; }
    public string? AnswerName { get; set; }
    public string? ImageName { get; set; }
    public string? ImagePath { get; set; }
    public string? ImageAlt { get; set; }
    public string? Email { get; set; }
    public int? UserId { get; set; }
    public int? BlogId { get; set; }
    public int? EmployeeId { get; set; }
}
