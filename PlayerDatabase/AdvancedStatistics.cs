namespace DataBase;

public record AdvancedStatistics(
    int PlayerId,
    double OffensiveRating = 0,
    double DefensiveRating = 0,
    double NetRating = 0,
    double AssistPercentage = 0,
    double AssistToTurnoverRatio = 0,
    double AssistRatio = 0,
    double OffensiveReboundsPercentage = 0,
    double DefensiveReboundsPercentage = 0,
    double ReboundsPercentage = 0,
    double TurnoverRatio = 0,
    double EffectiveFieldGoalsPercentage = 0,
    double TrueShootingPercentage = 0,
    double UsagePercentage = 0,
    double Pace = 0,
    double PlayerImpactEstimate = 0
);