namespace DB.Models;

public class Match
{
    public int Id { get; set; } 
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Location { get; set; }

    public int HomeTeamId { get; set; } 
    public Team HomeTeam { get; set; } 

    public int AwayTeamId { get; set; } 
    public Team AwayTeam { get; set; } 

    public string FinalScore { get; set; }
}