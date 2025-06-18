using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class Author
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string Bio { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public List<Book> Books { get; set; } = new List<Book>();
}