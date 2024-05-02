namespace DB.Models;

public class Player
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public PlayerPosition Position { get; set; }

    public int TeamId { get; set; } 
    public Team Team { get; set; } 

}