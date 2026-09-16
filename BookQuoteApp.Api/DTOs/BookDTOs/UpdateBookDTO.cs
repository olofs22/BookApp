namespace BookQuoteApp.Api.DTOs
{
    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public int? PublisherId { get; set; }
    }
}
