namespace ECommerce.API.DataTransferObject.Keywords
{
    public class KeywordDto : BaseDto, IKeywordDto
    {
        public string? KeywordText { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime CreatorUserId { get; set; } = DateTime.Now;
    }

}
