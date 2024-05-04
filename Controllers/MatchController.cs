// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using DB;
// using DB.Models.Enums;
// using HockeyStatsTracker.Models;
//
// namespace Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class MatchesController : ControllerBase
// {
//     private readonly DatabaseContext _context;
//
//     public MatchesController(DatabaseContext context)
//     {
//         _context = context;
//     }
//
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<MatchResultFE>>> GetMatches()
//     {
//         var matches = await _context.Matches
//             .Include(match => match.HomeTeam)
//             .Include(match => match.AwayTeam)
//             .Include(match => match.Goals)
//             .ToListAsync();
//
//         var matchResults = matches.Select(m => new MatchResultFE
//         {
//             Id = m.Id,
//             HomeTeam = m.HomeTeam.Name,
//             AwayTeam = m.AwayTeam.Name,
//             HomeGoals = m.Goals.Count(g => g.IsHomeTeamGoal),
//             AwayGoals = m.Goals.Count(g => !g.IsHomeTeamGoal),
//             Suffix = m.MatchResult switch
//             {
//                 MatchResult.HomeOTWin => "OT",
//                 MatchResult.AwayOTWin => "OT",
//                 MatchResult.HomeSOWin => "SO",
//                 MatchResult.AwaySOWin => "SO",
//                 _ => ""
//             },
//             Date = m.Date
//         });
//
//         return Ok(matchResults);
//     }
// }