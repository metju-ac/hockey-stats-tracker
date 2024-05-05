using DB;
using DB.Models.Enums;
using HockeyStatsTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StandingsController : Controller
{
    private readonly DatabaseContext _context;

    public StandingsController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("season/{seasonId:int}")]
    public async Task<ActionResult<IEnumerable<TeamStandingFE>>> GetStandingsBySeason(int seasonId)
    {
        var matches = await _context.Matches
            .Include(match => match.HomeTeam)
            .Include(match => match.AwayTeam)
            .Include(match => match.Goals)
            .Where(match => match.SeasonId == seasonId)
            .ToListAsync();

        var teams = matches.SelectMany(m => new[] { m.HomeTeam, m.AwayTeam }).Distinct();

        var teamStandings = teams.ToDictionary(t => t.Id, t => new TeamStandingFE
        {
            TeamId = t.Id,
            TeamName = t.Name,
            Wins = 0,
            Losses = 0,
            OTWins = 0,
            OTLosses = 0,
            ShootoutWins = 0,
            ShootoutLosses = 0,
            GoalsFor = 0,
            GoalsAgainst = 0,
            Points = 0
        });

        foreach (var match in matches)
        {
            var homeGoals = match.Goals.Count(g => g.IsHomeTeamGoal);
            var awayGoals = match.Goals.Count(g => !g.IsHomeTeamGoal);

            teamStandings[match.HomeTeamId].GoalsFor += homeGoals;
            teamStandings[match.HomeTeamId].GoalsAgainst += awayGoals;
            teamStandings[match.AwayTeamId].GoalsFor += awayGoals;
            teamStandings[match.AwayTeamId].GoalsAgainst += homeGoals;

            if (match.MatchResult == MatchResult.HomeWin)
            {
                teamStandings[match.HomeTeamId].Wins++;
                teamStandings[match.AwayTeamId].Losses++;
                teamStandings[match.HomeTeamId].Points += 3;
            }
            else if (match.MatchResult == MatchResult.AwayWin)
            {
                teamStandings[match.HomeTeamId].Losses++;
                teamStandings[match.AwayTeamId].Wins++;
                teamStandings[match.AwayTeamId].Points += 3;
            }
            else if (match.MatchResult == MatchResult.HomeOTWin)
            {
                teamStandings[match.HomeTeamId].OTWins++;
                teamStandings[match.HomeTeamId].Points += 2;
                teamStandings[match.AwayTeamId].OTLosses++;
                teamStandings[match.AwayTeamId].Points++;
            }
            else if (match.MatchResult == MatchResult.AwayOTWin)
            {
                teamStandings[match.AwayTeamId].OTWins++;
                teamStandings[match.AwayTeamId].Points += 2;
                teamStandings[match.HomeTeamId].OTLosses++;
                teamStandings[match.HomeTeamId].Points++;
            }
            else if (match.MatchResult == MatchResult.HomeSOWin)
            {
                teamStandings[match.HomeTeamId].ShootoutWins++;
                teamStandings[match.HomeTeamId].Points += 2;
                teamStandings[match.AwayTeamId].ShootoutLosses++;
                teamStandings[match.AwayTeamId].Points++;
            }
            else if (match.MatchResult == MatchResult.AwaySOWin)
            {
                teamStandings[match.AwayTeamId].ShootoutWins++;
                teamStandings[match.AwayTeamId].Points += 2;
                teamStandings[match.HomeTeamId].ShootoutLosses++;
                teamStandings[match.HomeTeamId].Points++;
            }
        }

        return Ok(teamStandings.Values);
    }
}