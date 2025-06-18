using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class Book
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public int? AuthorId { get; set; }
    public Author? AuthorEntity { get; set; }
    public DateTime createdAt { get; set; } 
}