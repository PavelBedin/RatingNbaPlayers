namespace DataBase;

public record TraditionalStatistics(
    int PlayerId,
    int GamePlayed = 0,
    double MinutesPlayed = 0,
    double PPG = 0, // Points Per Game
    double FGM = 0, // Field Goals Made 
    double FGA = 0, // Field Goals Attempted
    double FGP = 0, // Field Goals Percentage
    double TPM = 0, // 3 Point Field Goals Made
    double TPA = 0, // 3 Point Field Goals Attempted
    double TPP = 0, // 3 Point Field Goals Percentage
    double FTM = 0, // Free Throws Made
    double FTA = 0, // Free Throws Attempted
    double FTP = 0, // Free Throws Percentage
    double OREB = 0, // Offensive Rebounds
    double DRED = 0, // Defensive Rebounds
    double REB = 0, // Rebounds
    double AST = 0, // Assists
    double TOV = 0, // Turnovers
    double STL = 0, // Steals
    double BLK = 0, // Blocks
    double PF = 0, // Personal Fouls
    int DD2 = 0, // Double Doubles
    int TD3 = 0, // Triple Doubles
    double PM = 0 // Plus-Minus
)
{
    public TraditionalStatistics() : this(0)
    {
    }
}