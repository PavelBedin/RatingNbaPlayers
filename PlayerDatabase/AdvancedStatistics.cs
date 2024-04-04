namespace DataBase
{
    public record AdvancedStatistics(
        int PlayerId, // Player identifier
        double OFFRTG = 0, // Offensive efficiency rating
        double DEFRTG = 0, // Defensive efficiency rating
        double NETRTG = 0, // Net rating
        double ASTP = 0, // Percentage of assists
        double ASTTO = 0, // Assist to turnover ratio
        double ASTR = 0, // Assist ratio
        double OREBP = 0, // Offensive rebounds percentage
        double DREBP = 0, // Defensive rebounds percentage
        double REBP = 0, // Rebounds percentage
        double TOR = 0, // Turnover ratio
        double EFGP = 0, // Effective field goals percentage
        double TSP = 0, // True shooting percentage
        double USGP = 0, // Usage percentage
        double PACE = 0, // Pace
        double PIE = 0 // Player impact estimate
    ) : IStatisticsPlayer
    {
        public AdvancedStatistics() : this(0)
        {
        }

        private AdvancedStatistics(int id, double[] statistics) : this(
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
            statistics[14])
        {
        }

        public List<string?> ToList()
        {
            return GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(double) || p.PropertyType == typeof(int))
                .Select(p => p.GetValue(this)?.ToString())
                .Select(s => s.Replace(",", "."))
                .ToList();
        }

        public static IStatisticsPlayer Create(int id, double[] stat)
        {
            return new AdvancedStatistics(id, stat);
        }
    }
}