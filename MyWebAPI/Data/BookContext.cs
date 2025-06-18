using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Data;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationship between book and author
        modelBuilder.Entity<Book>()
            .HasOne(b => b.AuthorEntity)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.SetNull); // If author is deleted, set books' AuthorId to null
        
        // Seed Authors first
        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                Name = "Robert Martin",
                Bio = "Clean Code advocate and software craftsman",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0,  DateTimeKind.Utc)
            },
            new Author
            {
                Id = 2,
                Name = "Gang of Four",
                Bio = "Authors of the famous Design Patterns book",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0,  DateTimeKind.Utc)
            }
        );
        
        // Update books seed data to use AuthorId
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1, 
                Title = "Clean Code", 
                AuthorId = 1,
                createdAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new Book
            {
                Id = 2, 
                Title = "Design Patterns", 
                AuthorId = 2,
                createdAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}