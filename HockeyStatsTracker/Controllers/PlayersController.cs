using DB;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : Controller
{
    private readonly DatabaseContext _context;

    public PlayersController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("season/{seasonId:int}")]
    public async Task<ActionResult<IEnumerable<PlayerStandingFE>>> GetStandingsBySeason(int seasonId)
    {
        var matches = await _context.Matches
            .Where(match => match.SeasonId == seasonId)
            .Include(match => match.HomeTeam)
            .ThenInclude(team => team.Players)
            .Include(match => match.AwayTeam)
            .ThenInclude(team => team.Players)
            .Include(match => match.Goals).Include(match => match.Penalties)
            .ToListAsync();

        var players = matches.SelectMany(m => m.HomeTeam.Players.Concat(m.AwayTeam.Players)).Distinct();

        var playerStandings = players.Select(p => new PlayerStandingFE
        {
            PlayerId = p.Id,
            PlayerName = p.Name + " " + p.Surname,
            TeamName = matches.First(m => m.HomeTeam.Players.Contains(p) || m.AwayTeam.Players.Contains(p))
                .HomeTeam.Players.Contains(p) ? matches.First(m => m.HomeTeam.Players.Contains(p)).HomeTeam.Name : matches.First(m => m.AwayTeam.Players.Contains(p)).AwayTeam.Name,
            GamesPlayed = matches.Count(m => m.HomeTeam.Players.Contains(p) || m.AwayTeam.Players.Contains(p)),
            Goals = matches.Sum(m => m.Goals.Count(g => g.PlayerId == p.Id)),
            PenaltyMinutes = matches.Sum(m =>
                m.Penalties.Where(penalty => penalty.PlayerId == p.Id).Sum(penalty =>
                    PenaltyTypeExtensions.GetPenaltyMinutes(penalty.PenaltyType))),
        });

        return Ok(playerStandings);
    }
}