using System.Text.Json;
using GithubPortfolio.Data;
using GithubPortfolio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GithubPortfolio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GithubController : ControllerBase
{
    private readonly AppDbContext _context;

    public GithubController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("sync/{userId}")]
    public async Task<ActionResult> SyncUserRepos(int userId)
    {
        // Get user
        var user = await _context.Users
            .Include(u => u.Repositories)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        // Call GitHub API
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
        var response = await httpClient.GetStringAsync($"https://api.github.com/users/{user.GitHubUsername}/repos");
        
        // Parse JSON
        var gitHubRepos = JsonSerializer.Deserialize<List<GitHubRepo>>(response);
        
        // Clear existing repos and add new ones
        user.Repositories.Clear();
        foreach (var repo in gitHubRepos)
        {
            user.Repositories.Add(new Repository
            {
                Name = repo.name,
                Language = repo.language ?? "",
                Stars = repo.stargazers_count,
                UserId = userId
            });
        }
        
        // Save to database
        await _context.SaveChangesAsync();

        return Ok($"Synced {gitHubRepos.Count} repositories");
    }
    
}