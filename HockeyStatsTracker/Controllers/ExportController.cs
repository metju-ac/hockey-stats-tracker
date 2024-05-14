using System.Text.Json;
using System.Text.Json.Serialization;
using DB;
using DB.Models;
using DB.Models.Enums;
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
    
    [HttpGet]
    public async Task<IActionResult> ExportData()
    {
        Console.WriteLine("Exporting data");
        var filePath = Path.GetTempFileName();
        await ExportDataAsync(filePath);

        var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
        return Content(jsonData, "application/json");
        
    }
    
    private async Task ExportDataAsync(string filePath)
    {
        var matches = await _context.Matches.ToListAsync();
        var teams = await _context.Teams.ToListAsync();
        var players = await _context.Players.ToListAsync();
        var leagues = await _context.Leagues.ToListAsync();
        var seasons = await _context.Seasons.ToListAsync();
        var goals = await _context.Goals.ToListAsync();
        var penalties = await _context.Penalties.ToListAsync();

        var data = new
        {
            Matches = matches,
            Teams = teams,
            Players = players,
            Leagues = leagues,
            Seasons = seasons,
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