using PlayerDatabase;
using Rating;

class Program
{
    public static void Main()
    {
        var db = new Database();

        var tradStat = DataConverter.Convert(db.GetAllTraditionalStatistics());
        var advStat = DataConverter.Convert(db.GetAllAdvancedStatistics());
        var offensive = tradStat.Item1.Zip(advStat.Item1, (t, a) => t.Concat(a).ToArray()).ToArray();
        var defensive = tradStat.Item2.Zip(advStat.Item2, (t, a) => t.Concat(a).ToArray()).ToArray();

        var pcaWorker = new PCAWorker();
        var data = pcaWorker.MakePCA(offensive, defensive, true);
        var dictionaryRating = new Dictionary<int, int>();
        for (var i = 0; i < data.Count; i++)
        {
            // dictionaryRating.Add(i + 1,
            //     Rating(data[i], worstX, bestX + Math.Abs(worstX), worstY, bestY + Math.Abs(worstY)));
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