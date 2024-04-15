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
    double PF = 0 // Personal Fouls
) : IStatisticsPlayer
{
    public TraditionalStatistics() : this(0)
    {
    }

    private TraditionalStatistics(int id, double[] statistics) : this(
        id,
        (int)statistics[0],
        statistics[1],
        statistics[2],
        statistics[3],
        statistics[4],
        statistics[5],
        statistics[6],
        statistics[7],
        statistics[8],
        statistics[9],
        statistics[10],
        statistics[11],
        statistics[12],
        statistics[13],
        statistics[14],
        statistics[15],
        statistics[16],
        statistics[17],
        statistics[18],
        statistics[19])
    {
    }

    public List<string?> ToList()
    {
        return GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(double) || p.PropertyType == typeof(int))
            .Select(p => p.GetValue(this)?.ToString())
            .Select(s => s?.Replace(",", "."))
            .ToList();
    }

    public static IStatisticsPlayer Create(int id, double[] stat)
    {
        return new TraditionalStatistics(id, stat);
    }
}