namespace MyWebAPI.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public AuthorSummaryDto? Author { get; set; }
}

public class BookSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBookDto
{
    public string Title { get; set; } =  string.Empty;
    public int AuthorId { get; set; }
}

public class UpdateBookDto
{
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
}

public class UpdateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
}