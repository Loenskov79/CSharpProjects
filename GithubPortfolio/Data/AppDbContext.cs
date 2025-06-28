using Microsoft.EntityFrameworkCore;
using GithubPortfolio.Models; // tilføj namespace til dine models

namespace GithubPortfolio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Repository> Repositories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Loenskov",
                    Email = "loenskov@example.com",
                    Password = "password123",
                    GitHubUsername = "Loenskov79"
                },
                new User
                {
                    Id = 2,
                    Username = "kristian_user", 
                    Email = "kristian@example.com",
                    Password = "password123",
                    GitHubUsername = "Kristian"
                }
            );
        }
    }
}