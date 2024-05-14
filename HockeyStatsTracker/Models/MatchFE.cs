using DB.Models.Enums;

namespace HockeyStatsTracker.Models;

public class MatchFE
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    
    public string HomeTeamName { get; set; }
    public int HomeTeamId { get; set; }
    public string AwayTeamName { get; set; }
    public int AwayTeamId { get; set; }
    
    public int HomeGoalsCount { get; set; }
    public int AwayGoalsCount { get; set; }
    public string Suffix { get; set; }
    public MatchResult MatchResult { get; set; }
    
    public ICollection<GoalFE> HomeGoals { get; set; } = new List<GoalFE>();
    public ICollection<GoalFE> AwayGoals { get; set; } = new List<GoalFE>();
    
    public ICollection<PenaltyFE> HomePenalties { get; set; } = new List<PenaltyFE>();
    public ICollection<PenaltyFE> AwayPenalties { get; set; } = new List<PenaltyFE>();
}