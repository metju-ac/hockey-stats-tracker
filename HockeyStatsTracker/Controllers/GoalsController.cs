using DB;
using DB.Models;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalsController : Controller
{
    private readonly DatabaseContext _context;
    
    public GoalsController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpDelete("{goalId:int}")]
    public async Task<ActionResult> DeleteGoal(int goalId)
    {
        var goal = await _context.Goals.FindAsync(goalId);
        if (goal == null)
        {
            return NotFound();
        }

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private async Task<ActionResult<Goal>> AddGoal(int matchId, GoalFE goalFE, bool isHomeTeamGoal)
    {
        var timeParts = goalFE.TimeScored.Split(':');
        if (timeParts.Length != 2
            || !int.TryParse(timeParts[0], out var minutes)
            || !int.TryParse(timeParts[1], out var seconds)
            || seconds < 0 || seconds > 59)
        {
            return BadRequest("Invalid TimeScored format");
        }

        var timeScored = new TimeSpan(0, minutes, seconds);

        var goal = new Goal
        {
            MatchId = matchId,
            TimeScored = timeScored,
            ScorerId = goalFE.ScorerId,
            Assister1Id = goalFE.Assister1Id,
            Assister2Id = goalFE.Assister2Id,
            IsHomeTeamGoal = isHomeTeamGoal
        };

        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost("home/{matchId:int}")]
    public async Task<ActionResult<Goal>> AddHomeGoal(int matchId, GoalFE goalFE)
    {
        return await AddGoal(matchId, goalFE, true);
    }
    
    [HttpPost("away/{matchId:int}")]
    public async Task<ActionResult<Goal>> AddAwayGoal(int matchId, GoalFE goalFE)
    {
        return await AddGoal(matchId, goalFE, false); 
    }
    
    
}