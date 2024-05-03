namespace DB.Models;

public class Season
{
    public int Id { get; set; }
    public int Year { get; set; } 

    public int LeagueId { get; set; } 
    public League League { get; set; } 

    public ICollection<Team> Teams { get; set; } 
}