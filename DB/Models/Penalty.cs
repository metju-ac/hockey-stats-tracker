using DB.Models.Enums;

namespace DB.Models;

public class Penalty
{
    public int Id { get; set; }
    public PenaltyType PenaltyType { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }
    public bool IsHomeTeamPenalty { get; set; }

    public int MatchId { get; set; }
    public Match Match { get; set; }

    public TimeSpan TimeTaken { get; set; }

}