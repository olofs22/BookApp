namespace BookQuoteApp.Api.DTOs
{
    public class QuoteDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
