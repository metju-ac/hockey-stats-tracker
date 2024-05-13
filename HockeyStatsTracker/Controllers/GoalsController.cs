using DB;
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
}