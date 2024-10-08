using DB;
using DB.Models;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : Controller
{
    private readonly DatabaseContext _context;
    
    public MatchesController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchPreviewFE>>> GetMatches()
    {
        var matches = await _context.Matches
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .Include(match => match.Goals)
            .ToListAsync();
        
        
        var matchResults = matches.Select(m => new MatchPreviewFE
        {
            Id = m.Id,
            HomeTeam = m.HomeTeam.Name,
            AwayTeam = m.AwayTeam.Name,
            HomeGoals = m.Goals.Count(g => g.IsHomeTeamGoal),
            AwayGoals = m.Goals.Count(g => !g.IsHomeTeamGoal),
            Suffix = m.MatchResult switch
            {
                MatchResult.HomeOTWin => "OT",
                MatchResult.AwayOTWin => "OT",
                MatchResult.HomeSOWin => "SO",
                MatchResult.AwaySOWin => "SO",
                _ => ""
            },
            Date = m.Date
        });

        return Ok(matchResults);
    }
    
    [HttpGet("season/{seasonId:int}")]
    public async Task<ActionResult<IEnumerable<MatchPreviewFE>>> GetMatchesBySeason(int seasonId)
    {
        var matches = await _context.Matches
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .Include(match => match.Goals)
            .Where(match => match.SeasonId == seasonId)
            .ToListAsync();

        var matchResults = matches.Select(m => new MatchPreviewFE
        {
            Id = m.Id,
            HomeTeam = m.HomeTeam.Name,
            AwayTeam = m.AwayTeam.Name,
            HomeGoals = m.Goals.Count(g => g.IsHomeTeamGoal),
            AwayGoals = m.Goals.Count(g => !g.IsHomeTeamGoal),
            Suffix = m.MatchResult switch
            {
                MatchResult.HomeOTWin => "OT",
                MatchResult.AwayOTWin => "OT",
                MatchResult.HomeSOWin => "SO",
                MatchResult.AwaySOWin => "SO",
                _ => ""
            },
            Date = m.Date
        });

        return Ok(matchResults);
    }
    
    [HttpGet("{matchId:int}")]
    public async Task<ActionResult<MatchFE>> GetMatchById(int matchId)
    {
        var match = await _context.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Goals).ThenInclude(goal => goal.Scorer).Include(match => match.Penalties)
            .ThenInclude(penalty => penalty.Player)
            .FirstOrDefaultAsync(m => m.Id == matchId);

        if (match == null)
        {
            return NotFound();
        }

        var matchFE = new MatchFE
        {
            Id = match.Id,
            Date = match.Date,
            Location = match.Location,  
            
            HomeTeamName = match.HomeTeam.Name,
            HomeTeamId = match.HomeTeamId,
            AwayTeamName = match.AwayTeam.Name,
            AwayTeamId = match.AwayTeamId,
            
            HomeGoalsCount = match.Goals.Count(g => g.IsHomeTeamGoal),
            AwayGoalsCount = match.Goals.Count(g => !g.IsHomeTeamGoal),
            Suffix = match.MatchResult switch
            {
                MatchResult.HomeOTWin => "OT",
                MatchResult.AwayOTWin => "OT",
                MatchResult.HomeSOWin => "SO",
                MatchResult.AwaySOWin => "SO",
                _ => ""
            },
            MatchResult = match.MatchResult,
            
            HomeGoals = match.Goals
                .Where(g => g.IsHomeTeamGoal)
                .OrderBy(g => g.TimeScored)
                .Select(g => new GoalFE
                {
                    Id = g.Id,
                    TimeScored = ((int)g.TimeScored.TotalMinutes).ToString("D2") + ":" + g.TimeScored.Seconds.ToString("D2"),
                    ScorerName = g.Scorer.Name + " " + g.Scorer.Surname,
                    Assister1Id = g.Assister1Id,
                    Assister1Name = g.Assister1 != null ? g.Assister1.Name + " " + g.Assister1.Surname : null,
                    Assister2Id = g.Assister2Id,
                    Assister2Name = g.Assister2 != null ? g.Assister2.Name + " " + g.Assister2.Surname : null,
                })
                .ToList(),
            
            AwayGoals = match.Goals
                .Where(g => !g.IsHomeTeamGoal)
                .OrderBy(g => g.TimeScored)
                .Select(g => new GoalFE
                {
                    Id = g.Id,
                    TimeScored = ((int)g.TimeScored.TotalMinutes).ToString("D2") + ":" + g.TimeScored.Seconds.ToString("D2"),
                    ScorerName = g.Scorer.Name + " " + g.Scorer.Surname,
                    Assister1Id = g.Assister1Id,
                    Assister1Name = g.Assister1 != null ? g.Assister1.Name + " " + g.Assister1.Surname : null,
                    Assister2Id = g.Assister2Id,
                    Assister2Name = g.Assister2 != null ? g.Assister2.Name + " " + g.Assister2.Surname : null,
                })
                .ToList(),
            
            HomePenalties = match.Penalties
                .Where(p => p.IsHomeTeamPenalty)
                .OrderBy(p => p.TimeTaken)
                .Select(p => new PenaltyFE
                {
                    Id = p.Id,
                    TimePenalty = ((int)p.TimeTaken.TotalMinutes).ToString("D2") + ":" + p.TimeTaken.Seconds.ToString("D2"),
                    PlayerName = p.Player.Name + " " + p.Player.Surname,
                    PenaltyType = p.PenaltyType.ToFormattedString(),
                    PenaltyMinutes = PenaltyTypeExtensions.GetPenaltyMinutes(p.PenaltyType)
                })
                .ToList(),
            
            AwayPenalties = match.Penalties
                .Where(p => !p.IsHomeTeamPenalty)
                .OrderBy(p => p.TimeTaken)
                .Select(p => new PenaltyFE
                {
                    Id = p.Id,
                    TimePenalty = ((int)p.TimeTaken.TotalMinutes).ToString("D2") + ":" + p.TimeTaken.Seconds.ToString("D2"),
                    PlayerName = p.Player.Name + " " + p.Player.Surname,
                    PenaltyType = p.PenaltyType.ToFormattedString(),
                    PenaltyMinutes = PenaltyTypeExtensions.GetPenaltyMinutes(p.PenaltyType)
                })
                .ToList()
        };

        return Ok(matchFE);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMatch(int id, MatchFE matchFE)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        match.Location = matchFE.Location;
        match.Date = matchFE.Date;
        match.MatchResult = matchFE.MatchResult;

        _context.Entry(match).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult<MatchFE>> CreateMatch(MatchFE matchFE)
    {
        if (matchFE.HomeTeamId == matchFE.AwayTeamId)
        {
            return BadRequest("Home team and away team cannot be the same");
        }
        
        if (matchFE.SeasonId == null)
        {
            return BadRequest("SeasonId is required");
        }
        
        var match = new Match
        {
            SeasonId = (int)matchFE.SeasonId!,
            Date = matchFE.Date,
            Location = matchFE.Location,
            HomeTeamId = matchFE.HomeTeamId,
            AwayTeamId = matchFE.AwayTeamId,
            MatchResult = matchFE.MatchResult
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        return await GetMatchById(match.Id);
    }
    
    [HttpDelete("{matchId:int}")]
    public async Task<ActionResult> DeleteMatch(int matchId)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match == null)
        {
            return NotFound();
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}