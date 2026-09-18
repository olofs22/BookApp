using System.ComponentModel.DataAnnotations;

namespace BookQuoteApp.Api.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
