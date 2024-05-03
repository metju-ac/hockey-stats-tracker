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

        var teamDynamo = Dynamo();
        var teamOcelari = Ocelari();
        var teamSparta = Sparta();
        var teamLitvinov = Litvinov();

        context.Teams.AddRange(teamDynamo, teamOcelari, teamSparta, teamLitvinov);
        
        context.SaveChanges();
    }

    private static Team Dynamo()
    {
        var teamDynamo = new Team { Name = "HC Dynamo Pardubice" };
        
        teamDynamo.Players.Add(new Player
            { Name = "Lukáš", Surname = "Sedlák", BirthDate = new DateTime(1993, 2, 26), Position = PlayerPosition.Center, ShotSide = ShotSide.Left});
        teamDynamo.Players.Add(new Player
            { Name = "Tomáš", Surname = "Hyka", BirthDate = new DateTime(1990, 1, 1), Position = PlayerPosition.RightWing, ShotSide = ShotSide.Right});
        teamDynamo.Players.Add(new Player
            { Name = "Richard", Surname = "Pánik", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.LeftWing, ShotSide = ShotSide.Left});
        teamDynamo.Players.Add(new Player
            { Name = "Peter", Surname = "Čerešňák", BirthDate = new DateTime(1995, 2, 15), Position = PlayerPosition.RightDefensemen, ShotSide = ShotSide.Right});
        teamDynamo.Players.Add(new Player
            { Name = "David", Surname = "Musil", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.LeftDefensemen, ShotSide = ShotSide.Left});
        teamDynamo.Players.Add(new Player
            { Name = "Roman", Surname = "Will", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.Goaltender, ShotSide = ShotSide.Right});
        
        return teamDynamo;
    }
    
    private static Team Ocelari()
    {
        var teamOcelari = new Team { Name = "HC Oceláři Třinec" };
        
        teamOcelari.Players.Add(new Player
            { Name = "Martin", Surname = "Růžička", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightWing, ShotSide = ShotSide.Right});
        teamOcelari.Players.Add(new Player
            { Name = "David", Surname = "Cienciala", BirthDate = new DateTime(1999, 11, 27), Position = PlayerPosition.Center, ShotSide = ShotSide.Left});
        teamOcelari.Players.Add(new Player
            { Name = "Daniel", Surname = "Vozenilek", BirthDate = new DateTime(1990, 1, 1), Position = PlayerPosition.LeftWing, ShotSide = ShotSide.Left});
        teamOcelari.Players.Add(new Player
            { Name = "Jakub", Surname = "Jeřábek", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.LeftDefensemen, ShotSide = ShotSide.Left});
        teamOcelari.Players.Add(new Player
            { Name = "Martin", Surname = "Marinčin", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightDefensemen, ShotSide = ShotSide.Right});
        teamOcelari.Players.Add(new Player
            { Name = "Ondřej", Surname = "Kacetl", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.Goaltender, ShotSide = ShotSide.Right});
        
        return teamOcelari;
    }
    
    private static Team Sparta()
    {
        var teamSparta = new Team { Name = "HC Sparta Praha" };
        
        teamSparta.Players.Add(new Player
            { Name = "Pavel", Surname = "Kousal", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightWing, ShotSide = ShotSide.Right});
        teamSparta.Players.Add(new Player
            { Name = "Ondřej", Surname = "Najman", BirthDate = new DateTime(1999, 11, 27), Position = PlayerPosition.Center, ShotSide = ShotSide.Left});
        teamSparta.Players.Add(new Player
            { Name = "Michal", Surname = "Řepík", BirthDate = new DateTime(1990, 1, 1), Position = PlayerPosition.LeftWing, ShotSide = ShotSide.Left});
        teamSparta.Players.Add(new Player
            { Name = "Jakub", Surname = "Krejčík", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.LeftDefensemen, ShotSide = ShotSide.Left});
        teamSparta.Players.Add(new Player
            { Name = "Michal", Surname = "Kempný", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.RightDefensemen, ShotSide = ShotSide.Right});
        teamSparta.Players.Add(new Player
            { Name = "Jakub", Surname = "Kovář", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.Goaltender, ShotSide = ShotSide.Right});

        return teamSparta;
    }
    
    private static Team Litvinov()
    {
        var teamLitvinov = new Team { Name = "HC Verva Litvínov" };
        
        teamLitvinov.Players.Add(new Player
            { Name = "Ondřej", Surname = "Kaše", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.LeftWing, ShotSide = ShotSide.Left});
        teamLitvinov.Players.Add(new Player
            { Name = "David", Surname = "Kaše", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.Center, ShotSide = ShotSide.Right});
        teamLitvinov.Players.Add(new Player
            { Name = "Nicolas", Surname = "Hlava", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.RightWing, ShotSide = ShotSide.Right});
        teamLitvinov.Players.Add(new Player
            { Name = "Radim", Surname = "Šalda", BirthDate = new DateTime(1991, 3, 3), Position = PlayerPosition.LeftDefensemen, ShotSide = ShotSide.Left});
        teamLitvinov.Players.Add(new Player
            { Name = "Janis", Surname = "Jaks", BirthDate = new DateTime(1993, 4, 5), Position = PlayerPosition.RightDefensemen, ShotSide = ShotSide.Right});
        teamLitvinov.Players.Add(new Player
            { Name = "Matej", Surname = "Tomek", BirthDate = new DateTime(1992, 5, 7), Position = PlayerPosition.Goaltender, ShotSide = ShotSide.Right});
        
        return teamLitvinov;
    }
}