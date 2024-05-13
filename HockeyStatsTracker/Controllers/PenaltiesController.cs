using DB;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PenaltiesController : Controller
{
    private readonly DatabaseContext _context;
    
    public PenaltiesController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpDelete("{penaltyId:int}")]
    public async Task<ActionResult> DeletePenalty(int penaltyId)
    {
        var penalty = await _context.Penalties.FindAsync(penaltyId);
        if (penalty == null)
        {
            return NotFound();
        }

        _context.Penalties.Remove(penalty);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
