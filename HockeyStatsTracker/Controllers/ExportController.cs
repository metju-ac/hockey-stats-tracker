using System.Text.Json;
using System.Text.Json.Serialization;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : Controller
{
    private readonly DatabaseContext _context;
    
    public ExportController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpGet("season/{seasonId:int}")]
    public async Task<IActionResult> ExportData(int seasonId)
    {
        var filePath = Path.GetTempFileName();
        await ExportDataAsyncBySeason(filePath, seasonId);

        var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
        return Content(jsonData, "application/json");
        
    }
    
    private async Task ExportDataAsyncBySeason(string filePath, int seasonId)
    {
        var matches = await _context.Matches
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .Include(match => match.Goals)
            .Include(match => match.Penalties)
            .Where(match => match.SeasonId == seasonId)
            .ToListAsync();
        
        var teams = matches.SelectMany(m => new[] { m.HomeTeam, m.AwayTeam }).Distinct().ToList();
        var players = teams.SelectMany(t => t.Players).Distinct().ToList();  
        var goals = matches.SelectMany(m => m.Goals).Distinct().ToList();
        var penalties = matches.SelectMany(m => m.Penalties).Distinct().ToList();
        
        var data = new
        {
            Matches = matches,
            Teams = teams,
            Players = players,
            Goals = goals,
            Penalties = penalties
        };
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var jsonData = JsonSerializer.Serialize(data, options);

        await System.IO.File.WriteAllTextAsync(filePath, jsonData);
    }
}