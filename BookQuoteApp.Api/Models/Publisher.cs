namespace BookQuoteApp.Api.Models
{
    public class Publisher
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Quote> Quotes { get; set; }
    }
}
