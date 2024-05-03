namespace DB.Models;

public class Goal
{
    public int Id { get; set; }
    public TimeSpan TimeScored { get; set; }

    public int PlayerId { get; set; }
    public Player Scorer { get; set; } 
    public bool IsHomeTeamGoal { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; }
}