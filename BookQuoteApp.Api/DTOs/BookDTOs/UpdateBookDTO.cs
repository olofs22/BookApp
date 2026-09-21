namespace BookQuoteApp.Api.DTOs
{
    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishDate { get; set; }
        public string? PublisherName { get; set; }
    }
}
