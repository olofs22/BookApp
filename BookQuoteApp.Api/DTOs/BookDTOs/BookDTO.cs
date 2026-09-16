namespace BookQuoteApp.Api.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
