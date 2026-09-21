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

    public class PublisherController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public PublisherController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var publishers = await _dbContext.Publisher.ToListAsync();
            var publisherDto = publishers.Select(p => new PublisherDTO
            {
                Id = p.Id,
                Name = p.Name
            });
            return Ok(publisherDto);
        }
    }
}
