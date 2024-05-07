namespace DB.Models.Enums;

public enum PenaltyType
{
    HighSticking,
    Tripping,
    Hooking,
    Holding,
    Interference,
    Roughing,
    Slashing,
    Boarding,
    Fighting
}

public static class PenaltyTypeExtensions
{
    public static int GetPenaltyMinutes(PenaltyType penaltyType)
    {
        return penaltyType switch
        {
            PenaltyType.HighSticking => 4,
            PenaltyType.Fighting => 5,
            _ => 2,
        };
    }
}