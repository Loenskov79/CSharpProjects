﻿namespace GithubPortfolio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GitHubUsername { get; set; }

        public List<Repository> Repositories { get; set; } = new();
    }
}