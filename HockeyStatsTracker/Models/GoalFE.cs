namespace HockeyStatsTracker.Models;

public class GoalFE
{
    public int Id { get; set; }
    public string TimeScored { get; set; }
    public int ScorerId { get; set; }
    public string? ScorerName { get; set; }
    public int? Assister1Id { get; set; }
    public string? Assister1Name { get; set; }
    public int? Assister2Id { get; set; }
    public string? Assister2Name { get; set; }
}