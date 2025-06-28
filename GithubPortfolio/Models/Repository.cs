using System.Text.Json.Serialization;

namespace GithubPortfolio.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public int Stars { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}