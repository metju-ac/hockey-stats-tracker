namespace HockeyStatsTracker.Models;

public class TeamStandingFE
{
    public int TeamId { get; set; }
    public string TeamName { get; set; }
    public int GamesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int OTWins { get; set; }
    public int OTLosses { get; set; }
    public int ShootoutWins { get; set; }
    public int ShootoutLosses { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int Points { get; set; }
}