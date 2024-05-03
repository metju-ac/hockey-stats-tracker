using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class DatabaseContext : DbContext
{

    
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Season> Seasons { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tmp_db.db");
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Match>()
    //         .HasMany(m => m.HomeGoals)
    //         .WithOne(g => g.Match)
    //         .HasForeignKey(g => g.MatchId)
    //         .HasConstraintName("FK_HomeTeamGoals_Match");
    //
    //     modelBuilder.Entity<Match>()
    //         .HasMany(m => m.AwayGoals)
    //         .WithOne(g => g.Match)
    //         .HasForeignKey(g => g.MatchId)
    //         .HasConstraintName("FK_AwayTeamGoals_Match");
    // }
}