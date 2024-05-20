namespace Rating;

using PlayerDatabase;

public class DataConverter
{
    public static Tuple<double[][], double[][]> Convert(IEnumerable<IStatisticsPlayer> data)
    {
        var k = data.First() switch
        {
            TraditionalStatistics => 1,
            AdvancedStatistics => 0,
            _ => 0
        };

        var off = FormingUnifiedData(data.Select(x => x.GetOffensiveStat()).ToArray(), k);
        var def = FormingUnifiedData(data.Select(x => x.GetDefensiveStat()).ToArray(), k);

        return Tuple.Create(off, def);
    }

    private static double[][] FormingUnifiedData(double[][] data, int k)
    {
        var length = data[0].Length;
        for (var i = 0; i < length; i++)
        {
            var max = data.Select(x => x[i]).Max();
            var min = data.Select(x => x[i]).Min();
            foreach (var d in data)
            {
                d[i] = Unification(max, min, d[i], i < length - k);
            }
        }

        return data;
    }

    private static double Unification(double max, double min, double value, bool isUp)
    {
        return isUp ? (value - min) / (max - min) : (max - value) / (max - min);
    }
}