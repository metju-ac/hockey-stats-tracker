using DB.Models;
using DB.Models.Enums;

namespace DB.Utils;

public static class DatabaseSeeder
{
    
    public static void ResetDatabase(DatabaseContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        SeedData(context);
    }
    public static void SeedData(DatabaseContext context)
    {
        if (context.Teams.Any()) return;
        
        var teamDynamo = new Team { Name = "HC Dynamo Pardubice" };
        var teamOcelari = new Team { Name = "HC Oceláři Třinec" };
        var teamSparta = new Team { Name = "HC Sparta Praha" };
        var teamLitvinov = new Team { Name = "HC Verva Litvínov" };

        context.Teams.AddRange(teamDynamo, teamOcelari, teamSparta, teamLitvinov);
        teamDynamo.Players.Add(new Player
            { Name = "Lukáš", Surname = "Sedlák", BirthDate = new DateTime(1990, 1, 1), Position = PlayerPosition.Center });
        teamDynamo.Players.Add(new Player
            { Name = "Peter", Surname = "Čerešňák", BirthDate = new DateTime(1995, 2, 15), Position = PlayerPosition.RightDefensemen });
        teamDynamo.Players.Add(new Player
            { Name = "Roman", Surname = "Will", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.Goaltender });
        
        teamOcelari.Players.Add(new Player
            { Name = "Martin", Surname = "Růžička", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightWing });
        teamOcelari.Players.Add(new Player
            { Name = "Jakub", Surname = "Jeřábek", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.LeftDefensemen });
        teamOcelari.Players.Add(new Player
            { Name = "Ondřej", Surname = "Kacetl", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.Goaltender });
        
        teamSparta.Players.Add(new Player
            { Name = "Michal", Surname = "Řepík", BirthDate = new DateTime(1990, 1, 1), Position = PlayerPosition.Center });
        teamSparta.Players.Add(new Player
            { Name = "David", Surname = "Němeček", BirthDate = new DateTime(1995, 2, 15), Position = PlayerPosition.RightDefensemen });
        teamSparta.Players.Add(new Player
            { Name = "Jakub", Surname = "Kovář", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.Goaltender });
        
        teamLitvinov.Players.Add(new Player
            { Name = "Ondřej", Surname = "Kaše", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightWing });
        teamLitvinov.Players.Add(new Player
            { Name = "David", Surname = "Kaše", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.Center });
        teamLitvinov.Players.Add(new Player
            { Name = "Matej", Surname = "Tomek", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.Goaltender });
        
        context.SaveChanges();
    }
}