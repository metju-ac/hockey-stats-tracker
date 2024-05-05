using DB;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonsController : Controller
{
    private readonly DatabaseContext _context;
    
    public SeasonsController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeasonFE>>> GetSeasons()
    {
        var seasons = await _context.Seasons.ToListAsync();
        
        var seasonFEs = seasons.Select(s => new SeasonFE
        {
            Id = s.Id,
            Name = $"{s.Year - 1}/{s.Year}"
        });
        
        return Ok(seasonFEs);
    }
    
    [HttpGet("league/{leagueId:int}")]
    public async Task<ActionResult<IEnumerable<SeasonFE>>> GetSeasonsByLeague(int leagueId)
    {
        var seasons = await _context.Seasons
            .Where(season => season.LeagueId == leagueId)
            .ToListAsync();
        
        var seasonFEs = seasons.Select(s => new SeasonFE
        {
            Id = s.Id,
            Name = $"{s.Year - 1}/{s.Year}"
        });
        
        return Ok(seasonFEs);
    }
}