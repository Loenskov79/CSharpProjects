namespace MyWebAPI.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<BookSummaryDto> Books { get; set; } = new();
}

public class AuthorSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}

public class CreateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}