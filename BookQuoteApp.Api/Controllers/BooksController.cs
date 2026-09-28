using BookQuoteApp.Api.Data;
using BookQuoteApp.Api.DTOs;
using BookQuoteApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;


namespace BookQuoteApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }
        public BooksController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _dbContext.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b=> b.User).ToListAsync();
            var bookDtos = books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
                PublisherName = b.Publisher != null ? b.Publisher.Name : null,
                PublishDate = b.PublishDate,
                UserId = b.UserId,
                UserName = b.User?.Name
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
                PublishDate = book.PublishDate,
                UserId = book.UserId,
                UserName = book.User?.Name

            };

            return Ok(bookDTO); 

        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDTO cDTO)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == cDTO.AuthorName.ToLower());
            if(author == null)
            {
                author = new Author { Name = cDTO.AuthorName };
                _dbContext.Authors.Add(author);
                await _dbContext.SaveChangesAsync();
            }
            Publisher? publisher = null;
            if (!string.IsNullOrWhiteSpace(cDTO.PublisherName))
            {
                publisher = await _dbContext.Publisher.FirstOrDefaultAsync(p => p.Name.ToLower() == cDTO.PublisherName.ToLower());
                if (publisher == null)
                {
                    publisher = new Publisher { Name = cDTO.PublisherName };
                    _dbContext.Publisher.Add(publisher);
                    await _dbContext.SaveChangesAsync();
                }
            }

            var book = new Book
            {
                Title = cDTO.Title,
                AuthorId = author.Id,
                PublisherId = publisher?.Id,
                PublishDate = cDTO.PublishDate,
                UserId = GetUserId()
            };
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            var owner = await _dbContext.Users.FindAsync(book.UserId);

            var bookDto = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = author.Name,
                PublisherName = publisher?.Name,
                PublishDate = book.PublishDate,
                UserId = book.UserId,
                UserName = owner?.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, bookDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDTO uDTO)
        {
            var userId = GetUserId();

            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (book == null)
                return NotFound();

            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == uDTO.AuthorName.ToLower());
            if (author == null)
            {
                author = new Author { Name = uDTO.AuthorName };
                _dbContext.Authors.Add(author);
                await _dbContext.SaveChangesAsync();
            }

            Publisher? publisher = null;
                if(!string.IsNullOrWhiteSpace(uDTO.PublisherName)){
                publisher = await _dbContext.Publisher.FirstOrDefaultAsync(p => p.Name.ToLower() == uDTO.PublisherName.ToLower());
            }           
            if (publisher == null)
            {
                publisher = new Publisher { Name = uDTO.PublisherName };
                _dbContext.Publisher.Add(publisher);
                await _dbContext.SaveChangesAsync();
            }
            book.Title = uDTO.Title;
            book.AuthorId = author.Id;
            book.PublisherId = publisher?.Id;
            book.PublishDate = uDTO.PublishDate;


            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var userId = GetUserId();

            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (book == null)
                return NotFound();

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}


