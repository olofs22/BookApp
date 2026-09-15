namespace BookQuoteApp.Api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public DateTime PublishDate { get; set; }
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
