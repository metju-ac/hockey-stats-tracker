using DB;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaguesController : Controller 
{
    private readonly DatabaseContext _context;
    
    public LeaguesController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeagueFE>>> GetLeagues()
    {
        var leagues = await _context.Leagues.ToListAsync();
        
        var leagueFEs = leagues.Select(l => new LeagueFE
        {
            Id = l.Id,
            Name = l.Name
        });
        
        return Ok(leagueFEs);
    }
}