namespace DataBase;

public record TraditionalStatistics(
    int PlayerId,
    int GamesPlayed = 0,
    double MinutesPlayed = 0,
    double PointsPerGame = 0,
    double FieldGoalsMade = 0,
    double FieldGoalsAttempted = 0,
    double FieldGoalsPercentage = 0,
    double ThreePointFieldGoalsMade = 0,
    double ThreePointFieldGoalsAttempted = 0,
    double ThreePointFieldGoalsPercentage = 0,
    double FreeThrowsMade = 0,
    double FreeThrowsAttempted = 0,
    double FreeThrowsPercentage = 0,
    double OffensiveRebounds = 0,
    double DefensiveRebounds = 0,
    double TotalRebounds = 0,
    double Assists = 0,
    double Turnovers = 0,
    double Steals = 0,
    double Blocks = 0,
    double PersonalFouls = 0,
    int DoubleDoubles = 0,
    int TripleDoubles = 0,
    double PlusMinus = 0
);