namespace BookQuoteApp.Api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Quote> Quotes { get; set; }

    }
}
