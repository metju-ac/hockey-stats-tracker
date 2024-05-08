namespace HockeyStatsTracker.Models;

public class PlayerStandingFE
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public string TeamName { get; set; }
    public int GamesPlayed { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int Points { get; set; }
    public int PenaltyMinutes { get; set; }
}