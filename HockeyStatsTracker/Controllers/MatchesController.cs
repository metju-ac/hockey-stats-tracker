using DB;
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
            
            HomeGoals = match.Goals
                .Where(g => g.IsHomeTeamGoal)
                .Select(g => new GoalFE
                {
                    Id = g.Id,
                    TimeScored = g.TimeScored,
                    ScorerId = g.Scorer.Id,
                    ScorerName = g.Scorer.Name,
                })
                .ToList(),
            
            AwayGoals = match.Goals
                .Where(g => !g.IsHomeTeamGoal)
                .Select(g => new GoalFE
                {
                    Id = g.Id,
                    TimeScored = g.TimeScored,
                    ScorerId = g.Scorer.Id,
                    ScorerName = g.Scorer.Name,
                })
                .ToList(),
            
            HomePenalties = match.Penalties
                .Where(p => p.IsHomeTeamPenalty)
                .Select(p => new PenaltyFE
                {
                    Id = p.Id,
                    TimePenalty = p.TimeTaken,
                    PlayerId = p.PlayerId,
                    PlayerName = p.Player.Name,
                    PenaltyType = p.PenaltyType.ToString(),
                    PenaltyMinutes = PenaltyTypeExtensions.GetPenaltyMinutes(p.PenaltyType)
                })
                .ToList(),
            
            AwayPenalties = match.Penalties
                .Where(p => !p.IsHomeTeamPenalty)
                .Select(p => new PenaltyFE
                {
                    Id = p.Id,
                    TimePenalty = p.TimeTaken,
                    PlayerId = p.PlayerId,
                    PlayerName = p.Player.Name,
                    PenaltyType = p.PenaltyType.ToString(),
                    PenaltyMinutes = PenaltyTypeExtensions.GetPenaltyMinutes(p.PenaltyType)
                })
                .ToList()
        };

        return Ok(matchFE);
    }
}