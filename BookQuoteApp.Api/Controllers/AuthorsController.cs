using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.DTOs;

namespace BookQuoteApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthorsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _dbContext.Authors.ToListAsync();
            var authorDto = authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name
            });
            return Ok(authorDto);
        }
    }
}
