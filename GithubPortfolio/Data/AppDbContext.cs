using Microsoft.EntityFrameworkCore;
using GithubPortfolio.Models; // tilføj namespace til dine models

namespace GithubPortfolio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Repository> Repositories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}