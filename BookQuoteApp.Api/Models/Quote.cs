namespace BookQuoteApp.Api.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public DateTime UploadTime { get; set; }
        public Book? Book { get; set; }
        public int? BookId { get; set; }
    }
}
