using System.ComponentModel.DataAnnotations;
namespace BookQuoteApp.Api.DTOs
{
    public class LoginDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
