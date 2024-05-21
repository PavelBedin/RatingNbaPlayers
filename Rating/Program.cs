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
        var data = pcaWorker.MakePCA(offensive, defensive);
        var rating = MakeRating(data);
        var players = db.GetAllPlayers();
        var zipped = rating.Zip(players, (rating, player) => new { Rating = rating, Player = player }).ToArray();
        foreach (var value in zipped)
        {
            db.AddPlayerRating(new PlayerRating(value.Player.Name, value.Player.Id, value.Rating));
        }
    }


    private static List<int> MakeRating(List<double[]> data)
    {
        var variance = new List<double>();
        var scope = new List<double>();
        var listMin = new List<double>();

        for (var i = 0; i < data[0].Length; i++)
        {
            var block = data.Select(x => x[i]).ToArray();
            var avg = block.Average();
            variance.Add(block.Select(x => Math.Pow(x - avg, 2)).Sum() / block.Length);
            listMin.Add(block.Min());
            scope.Add(block.Max() - listMin.Last());
        }

        var varianceSum = variance.Sum();
        var factor = variance.Select(value => value / varianceSum).ToList();
        var rating = new List<int>();
        foreach (var value in data)
        {
            var result = 0.0;
            for (var i = 0; i < data[0].Length; i++)
            {
                result += factor[i] * (value[i] - listMin[i]) / scope[i] * 100;
            }

            rating.Add((int)Math.Round(result));
        }


        return rating;
    }
}