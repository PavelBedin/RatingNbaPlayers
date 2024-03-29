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
    )
    {
        public AdvancedStatistics() : this(0)
        {
        }
    }
}