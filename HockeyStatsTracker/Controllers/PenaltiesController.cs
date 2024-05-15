using DB;
using DB.Models;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;

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

    private async Task<ActionResult<Penalty>> AddPenalty(int matchId, PenaltyFE penaltyFE, bool isHomeTeamPenalty)
    {
        var timeParts = penaltyFE.TimePenalty.Split(':');
        if (timeParts.Length != 2
            || !int.TryParse(timeParts[0], out var minutes)
            || !int.TryParse(timeParts[1], out var seconds)
            || seconds < 0 || seconds > 59)
        {
            return BadRequest("Invalid TimePenalty format");
        }

        var timePenalty = new TimeSpan(0, minutes, seconds);

        if (penaltyFE.PenaltyTypeEnum == null)
        {
            return BadRequest("Invalid PenaltyType");
        }

        var penalty = new Penalty
        {
            MatchId = matchId,
            TimeTaken = timePenalty,
            PlayerId = penaltyFE.PlayerId,
            IsHomeTeamPenalty = isHomeTeamPenalty,
            PenaltyType = (PenaltyType)penaltyFE.PenaltyTypeEnum
        };

        _context.Penalties.Add(penalty);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost("home/{matchId:int}")]
    public async Task<ActionResult<Penalty>> AddHomePenalty(int matchId, PenaltyFE penaltyFE)
    {
        return await AddPenalty(matchId, penaltyFE, true);
    }
    
    [HttpPost("away/{matchId:int}")]
    public async Task<ActionResult<Penalty>> AddAwayPenalty(int matchId, PenaltyFE penaltyFE)
    {
        return await AddPenalty(matchId, penaltyFE, false); 
    }
}
