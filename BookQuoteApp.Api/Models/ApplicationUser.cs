using Microsoft.AspNetCore.Identity;

namespace BookQuoteApp.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
