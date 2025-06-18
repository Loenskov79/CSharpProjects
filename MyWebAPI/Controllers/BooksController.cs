using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Data;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookContext _context;

    public BooksController(BookContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        // Include the Author data when fetching books
        var books = await _context.Books
            .Include(b => b.AuthorEntity)
            .ToListAsync();
        
        var bookDtos = books.Select(book => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.createdAt,
            Author = book.AuthorEntity == null ? null : new AuthorSummaryDto()
            {
                Id = book.AuthorEntity.Id,
                Name = book.AuthorEntity.Name,
                Bio = book.AuthorEntity.Bio,
            }
        }).ToList();
        
        return bookDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        // Include the Author data when fetching a single book
        var book = await _context.Books
            .Include(b => b.AuthorEntity)
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (book == null) 
            return NotFound();

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.createdAt,
            Author = book.AuthorEntity == null
                ? null
                : new AuthorSummaryDto
                {
                    Id = book.AuthorEntity.Id,
                    Name = book.AuthorEntity.Name,
                    Bio = book.AuthorEntity.Bio,
                }
        };
        
        return bookDto;
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
    { 
        // Convert DTO to Entity
        var book = new Book
        {
            Title = createBookDto.Title,
            AuthorId = createBookDto.AuthorId,
            createdAt = DateTime.Now,
        };
        
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        
        // Load the author for the response
        await _context.Entry(book)
            .Reference(b => b.AuthorEntity)
            .LoadAsync();
        
        // Convert back to DTO
        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.createdAt,
            Author = book.AuthorEntity == null
                ? null
                : new AuthorSummaryDto
                {
                    Id = book.AuthorEntity.Id,
                    Name = book.AuthorEntity.Name,
                    Bio = book.AuthorEntity.Bio,
                }
        };
        
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updateBookDto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        // Update entity from DTO
        book.Title = updateBookDto.Title;
        book.AuthorId = updateBookDto.AuthorId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

}