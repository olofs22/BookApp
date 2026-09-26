using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.Models;
using BookQuoteApp.Api.DTOs.QuoteDTOs;
using BookQuoteApp.Api.DTOs;
using System.Security.Claims;

namespace BookQuoteApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public QuotesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var quotes = await _dbContext.Quotes
                .Include(q => q.Author)
                .Include(q => q.Book)
                .Where(q => q.UserId == userId)
                .ToListAsync();

            var quoteDtos = quotes.Select(q => new QuoteDTO
            {
                Id = q.Id,
                Text = q.Text,
                BookId = q.BookId,
                AuthorName = q.Author.Name,
                BookTitle = q.Book.Title,
                UploadTime = q.UploadTime,
            }).ToList();

            return Ok(quoteDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            var quote = await _dbContext.Quotes
                .Include(q => q.Author)
                .Include(q => q.Book)
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote == null)
                return NotFound();

            var quoteDto = new QuoteDTO
            {
                Id = quote.Id,
                Text = quote.Text,
                BookId = quote.BookId, 
                AuthorName = quote.Author.Name,
                BookTitle = quote.Book.Title,
                UploadTime = quote.UploadTime
            };
            return Ok(quoteDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuote(CreateQuoteDTO cqDTO)
        {
            var userId = GetUserId();

            var bookExists = await _dbContext.Books.AnyAsync(b => b.Id == cqDTO.BookId);
            if (!bookExists)
                return BadRequest("Book not found");

            var author = await _dbContext.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == cqDTO.AuthorName.ToLower());
            if(author == null)
            {
                author = new Author { Name = cqDTO.AuthorName };
                _dbContext.Authors.Add(author);
                await _dbContext.SaveChangesAsync();
            }
            var quote = new Quote
            {
                Text = cqDTO.Text,
                AuthorId = author.Id,
                BookId = cqDTO.BookId,
                UserId = userId,
                UploadTime = DateTime.UtcNow
            };
            _dbContext.Quotes.Add(quote);
            await _dbContext.SaveChangesAsync();

            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == cqDTO.BookId);

            var quoteDto = new QuoteDTO
            {
                Id = quote.Id,
                Text = quote.Text,
                BookId = quote.BookId,
                AuthorName = author.Name,
                BookTitle = book!.Title,
                UploadTime = quote.UploadTime
            };
            return CreatedAtAction(nameof(GetById), new { id = quote.Id }, quoteDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, UpdateQuoteDTO uqDTO)
        {
            var userId = GetUserId();

            var quote = await _dbContext.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote == null)
                return NotFound();

            var bookExists = await _dbContext.Books.AnyAsync(b => b.Id == uqDTO.BookId);
            if (!bookExists)
                return BadRequest("Book not found");

            var author = await _dbContext.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == uqDTO.AuthorName.ToLower());

            if (author == null)
            {
                author = new Author { Name = uqDTO.AuthorName };
                _dbContext.Authors.Add(author);
                await _dbContext.SaveChangesAsync();
            }

            quote.Text = uqDTO.Text;
            quote.AuthorId = author.Id;
            quote.BookId = uqDTO.BookId;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var userId = GetUserId();

            var quote = await _dbContext.Quotes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            if (quote == null)
                return NotFound();

            _dbContext.Quotes.Remove(quote);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
