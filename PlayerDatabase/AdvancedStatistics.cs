namespace DataBase
{
    public record AdvancedStatistics(
        int PlayerId, // Player identifier
        double PER = 0, // Player efficiency rating
        double TSP = 0, // True shooting percentage
        double TPAR = 0, // 3 point attempt rate
        double FTR = 0, // Free throw attempt rate
        double OREBP = 0, // Offensive rebounds percentage
        double DREBP = 0, // Defensive rebounds percentage
        double REBP = 0, // Rebounds percentage
        double ASTP = 0, // Percentage of assists
        double STLP = 0, // Percentage of steal
        double BLKP = 0, // Percentage of block
        double TOVP = 0, // Percentage of turnover
        double USGP = 0, // Usage percentage
        double OWS = 0, // Offensive win shares
        double DWS = 0, // Defensive win shares
        double WS = 0, // Win shares
        double WS48 = 0, // Win shares per 48 minutes
        double OBPM = 0, // Offensive box plus/minus
        double DBPM = 0, // Defensive box plus/minus
        double BPM = 0, // Box plus/minus
        double VORP = 0 // Value over replacement player
    ) : IStatisticsPlayer
    {
        public AdvancedStatistics() : this(0)
        {
        }

        private AdvancedStatistics(int id, double[] statistics) : this(
            id,
            statistics[0],
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
            return new AdvancedStatistics(id, stat);
        }
    }
}