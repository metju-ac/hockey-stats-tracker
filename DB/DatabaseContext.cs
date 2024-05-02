using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class DatabaseContext : DbContext
{

    
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tmp_db.db");
    }
}