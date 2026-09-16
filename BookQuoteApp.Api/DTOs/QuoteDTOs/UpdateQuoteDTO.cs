namespace BookQuoteApp.Api.DTOs.QuoteDTOs
{
    public class UpdateQuoteDTO
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }
    }
}
