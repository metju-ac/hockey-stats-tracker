namespace DB.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
    
    public ICollection<Season> Seasons { get; set; } = new List<Season>();
}