namespace GithubPortfolio.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public int Stars { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}