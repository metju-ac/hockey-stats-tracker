namespace DB.Models;

public class League
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    public ICollection<Season> Seasons { get; set; } = new List<Season>();
}