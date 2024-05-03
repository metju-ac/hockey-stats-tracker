using DB.Models.Enums;

namespace DB.Models;

public class Match
{
    public int Id { get; set; } 
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Location { get; set; }
    
    public int SeasonId { get; set; }
    public Season Season { get; set; }

    public int HomeTeamId { get; set; } 
    public Team HomeTeam { get; set; } 

    public int AwayTeamId { get; set; } 
    public Team AwayTeam { get; set; } 
    
    public ICollection<Goal> HomeGoals { get; set; }
    public ICollection<Goal> AwayGoals { get; set; }
    public MatchResult MatchResult { get; set; } 
    
    public ICollection<Penalty> HomePenalties { get; set; }
    public ICollection<Penalty> AwayPenalties { get; set; }
}