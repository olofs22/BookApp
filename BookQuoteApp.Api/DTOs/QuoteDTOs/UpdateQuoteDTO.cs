namespace BookQuoteApp.Api.DTOs.QuoteDTOs
{
    public class UpdateQuoteDTO
    {
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public int BookId { get; set; }
    }
}
