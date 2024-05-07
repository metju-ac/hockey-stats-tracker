namespace HockeyStatsTracker.Models;

public class PenaltyFE
{
    public int Id { get; set; }
    public string TimePenalty { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public string PenaltyType { get; set; }
    public int PenaltyMinutes { get; set; }
}