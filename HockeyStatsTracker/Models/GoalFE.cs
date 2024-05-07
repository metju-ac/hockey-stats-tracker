namespace HockeyStatsTracker.Models;

public class GoalFE
{
    public int Id { get; set; }
    public TimeSpan TimeScored { get; set; }
    public int ScorerId { get; set; }
    public string ScorerName { get; set; }
}