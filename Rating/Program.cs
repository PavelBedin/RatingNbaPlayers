using PlayerDatabase;
using Rating;

class Program
{
    public static void Main()
    {
        var db = new Database();
        var pcaWorker = new PCAWorker();
        var data = pcaWorker.MakePCA(db.GetAllTraditionalStatistics(), db.GetAllAdvancedStatistics(), true);
        var worstX = -data.Select(x => x[0]).Max();
        var bestX = -data.Select(x => x[0]).Min();
        var worstY = data.Select(x => x[1]).Min();
        var bestY = data.Select(x => x[1]).Max();
        var dictionaryRating = new Dictionary<int, int>();
        for (var i = 0; i < data.Count; i++)
        {
            dictionaryRating.Add(i + 1,
                Rating(data[i], worstX, bestX + Math.Abs(worstX), worstY, bestY + Math.Abs(worstY)));
        }

        dictionaryRating = dictionaryRating
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);

        foreach (var value in dictionaryRating)
        {
            Console.WriteLine($"{db.GetPlayerById(value.Key).Name} - {value.Value}");
        }
    }

    private static int Rating(double[] data, double worstX, double bestX, double worstY, double bestY)
    {
        return (int)Math.Round(
            Math.Abs(-data[0] + Math.Abs(worstX)) / bestX * 60 + (data[1] + Math.Abs(worstY)) / bestY * 40
            , MidpointRounding.ToEven);
    }
}