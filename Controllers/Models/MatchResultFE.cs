namespace HockeyStatsTracker.Models;

public class MatchResultFE
{
    public int Id { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public string Suffix { get; set; }
    public DateTime Date { get; set; }
}