using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.Models;
using BookQuoteApp.Api.DTOs;


namespace BookQuoteApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/controller")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BooksController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _dbContext.Books.Include(b => b.Author).Include(b => b.Publisher).ToListAsync();
            var bookDtos = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
                PublisherName = b.Publisher != null ? b.Publisher.Name : null,
                PublishDate = b.PublishDate
            }).ToList();

            return Ok(bookDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _dbContext.Books.Include(b => b.Author).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name,
                PublisherName = book.Publisher != null ? book.Publisher.Name : null,
                PublishDate = book.PublishDate
            };

            return Ok(bookDTO);
        }
    }
}
