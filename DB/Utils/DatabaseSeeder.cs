using DB.Models;
using DB.Models.Enums;
using Microsoft.EntityFrameworkCore;

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
        var league = new League { Name = "Czech Extraliga", Country = "Czech Republic" };
        context.Leagues.Add(league);
        
        var season22 = new Season { Year = 2022, League = league };
        var season23 = new Season { Year = 2023, League = league };
        context.Seasons.Add(season22);
        context.Seasons.Add(season23);
        
        
        SeedTeams(context);
        SeedMatches(context, season22);
        SeedMatches(context, season23);
    }

    private static void SeedMatches(DatabaseContext context, Season season)
    {
        var teams = context.Teams.Include(team => team.Players).ToList();
        var random = new Random();
        
        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = 0; j < teams.Count; j++)
            {
                if (i == j) continue;
                
                Team homeTeam = teams[i];
                Team awayTeam = teams[j];
                
                int homeGoals = random.Next(0, 5);
                int awayGoals = random.Next(0, 5);

                MatchResult result;
                if (homeGoals > awayGoals)
                {
                    result = MatchResult.HomeWin;
                }
                else if (homeGoals < awayGoals)
                {
                    result = MatchResult.AwayWin;
                }
                else
                {
                    homeGoals++;
                    result = MatchResult.HomeOTWin;
                }
                
                var match = new Match
                {
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    Date = DateTime.Now,
                    Time = TimeSpan.FromHours(random.Next(0, 24)),
                    Location = "Random location",
                    Season = season,
                    MatchResult = result
                };

                var homePlayers = homeTeam.Players.Where(p => p.Position != PlayerPosition.Goaltender).ToList();
                for (int k = 0; k < homeGoals; k++)
                {
                    var scorer = homePlayers[random.Next(0, homePlayers.Count)];
                    var time = TimeSpan.FromSeconds(random.Next(0, 3600));
                    var goal = new Goal { TimeScored = time, Scorer = scorer, IsHomeTeamGoal = true, Match = match };

                    var assistersCount = random.Next(0, 3);

                    if (assistersCount > 0)
                    {
                        Player potentialAssister;
                        do
                        {
                            potentialAssister = homePlayers[random.Next(0, homePlayers.Count)];
                        } while (potentialAssister == scorer);

                        goal.Assister1 = potentialAssister;

                        if (assistersCount > 1)
                        {
                            do
                            {
                                potentialAssister = homePlayers[random.Next(0, homePlayers.Count)];
                            } while (potentialAssister == scorer || potentialAssister == goal.Assister1);

                            goal.Assister2 = potentialAssister;
                        }
                    }   
                    match.Goals.Add(goal);
                }

                var awayPlayers = awayTeam.Players.Where(p => p.Position != PlayerPosition.Goaltender).ToList();
                for (int k = 0; k < awayGoals; k++)
                {
                    var scorer = awayPlayers[random.Next(0, awayPlayers.Count)];
                    var time = TimeSpan.FromSeconds(random.Next(0, 3600));

                    var goal = new Goal { TimeScored = time, Scorer = scorer, IsHomeTeamGoal = false, Match = match };

                    var assistersCount = random.Next(0, 3);

                    if (assistersCount > 0)
                    {
                        Player potentialAssister;
                        do
                        {
                            potentialAssister = awayPlayers[random.Next(0, awayPlayers.Count)];
                        } while (potentialAssister == scorer);

                        goal.Assister1 = potentialAssister;

                        if (assistersCount > 1)
                        {
                            do
                            {
                                potentialAssister = awayPlayers[random.Next(0, awayPlayers.Count)];
                            } while (potentialAssister == scorer || potentialAssister == goal.Assister1);

                            goal.Assister2 = potentialAssister;
                        }
                    }

                    match.Goals.Add(goal);
                }

                int homePenalties = random.Next(0, 10);
                for (int k = 0; k < homePenalties; k++)
                {
                    var penaltyType = (PenaltyType) random.Next(0, Enum.GetValues<PenaltyType>().Length);
                    var player = homePlayers[random.Next(0, homePlayers.Count)];
                    var time = TimeSpan.FromSeconds(random.Next(0, 3600));
                    match.Penalties.Add(new Penalty { PenaltyType = penaltyType, Player = player, IsHomeTeamPenalty = true, TimeTaken = time, Match = match });
                }

                int awayPenalties = random.Next(0, 10);
                for (int k = 0; k < awayPenalties; k++)
                {
                    var penaltyType = (PenaltyType) random.Next(0, Enum.GetValues<PenaltyType>().Length);
                    var player = awayPlayers[random.Next(0, awayPlayers.Count)];
                    var time = TimeSpan.FromSeconds(random.Next(0, 3600));
                    match.Penalties.Add(new Penalty { PenaltyType = penaltyType, Player = player, IsHomeTeamPenalty = false, TimeTaken = time, Match = match });
                }

                context.Matches.Add(match);
            }
        }

        context.SaveChanges();
    }
    
    private static void SeedTeams(DatabaseContext context)
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