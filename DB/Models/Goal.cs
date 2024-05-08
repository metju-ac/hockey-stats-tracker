namespace DB.Models;

public class Goal
{
    public int Id { get; set; }
    public TimeSpan TimeScored { get; set; }

    public int ScorerId { get; set; }
    public Player Scorer { get; set; } 
    public int? Assister1Id { get; set; }
    public Player? Assister1 { get; set; }
    public int? Assister2Id { get; set; }
    public Player? Assister2 { get; set; }
    
    public bool IsHomeTeamGoal { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; }
}