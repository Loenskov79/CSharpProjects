using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Data;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly BookContext _context;

    public AuthorsController(BookContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _context.Authors
            .Include(a => a.Books)
            .ToListAsync();

        var authorDtos = authors.Select(author => new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            CreatedAt = author.CreatedAt,
            Books = author.Books.Select(book => new BookSummaryDto
            {
                Id = book.Id,
                Title = book.Title,
                CreatedAt = book.createdAt
            }).ToList()
        }).ToList();

        return authorDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
        {
            return NotFound();
        }

        var authorDto = new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            CreatedAt = author.CreatedAt,
            Books = author.Books.Select(book => new BookSummaryDto
            {
                Id = book.Id,
                Title = book.Title,
                CreatedAt = book.createdAt
            }).ToList()
        };

        return authorDto;
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto createAuthorDto)
    {
        var author = new Author
        {
            Name = createAuthorDto.Name,
            Bio = createAuthorDto.Bio,
            CreatedAt = DateTime.UtcNow
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        var authorDto = new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            CreatedAt = author.CreatedAt,
            Books = new List<BookSummaryDto>()  // New author has no books
        };

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        author.Name = updateAuthorDto.Name;
        author.Bio = updateAuthorDto.Bio;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AuthorExists(int id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}