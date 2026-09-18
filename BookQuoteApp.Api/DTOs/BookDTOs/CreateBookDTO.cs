using BookQuoteApp.Api.Models;

namespace BookQuoteApp.Api.DTOs
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public int? PublisherId { get; set; }
    }
}
