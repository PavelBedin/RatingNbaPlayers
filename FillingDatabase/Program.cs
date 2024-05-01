using HtmlAgilityPack;
using PlayerDatabase;

namespace FillingDatabase;

using AllPaths;

internal delegate void AddStatistics(IStatisticsPlayer statistics);

internal delegate IStatisticsPlayer Create(int id, double[] stat);

public static class Program
{
    private static readonly Database Db = new();

    public static void Main()
    {
        var path = new FindPath();
        AddToDatabase(player => Db.AddTraditionalStatistics((TraditionalStatistics)player),
            HttpsRequests.GetTraditionalStatistics(),
            TraditionalStatistics.Create, path.GetFullPath("FieldsTradStat"));
        AddToDatabase(player => Db.AddAdvancedStatistics((AdvancedStatistics)player),
            HttpsRequests.GetAdvancedStatistics(),
            AdvancedStatistics.Create, path.GetFullPath("FieldsAdvStat"));
    }

    private static void AddToDatabase(AddStatistics statisticsDelegate, HtmlNodeCollection collection,
        Create createDelegate, string path)
    {
        try
        {
            if (collection == null)
                throw new NullReferenceException();
            var players = new HashSet<string>();
            foreach (var node in collection)
            {
                var playerName = node.SelectSingleNode(".//td[@data-stat='player']")?.InnerText;
                if (playerName == null || players.Contains(playerName))
                    continue;
                var id = Db.AddPlayer(FormingName(playerName));
                players.Add(playerName);
                statisticsDelegate(createDelegate(id, FormingStat(node, path)));
            }
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
        }
    }

    private static double[] FormingStat(HtmlNode node, string path)
    {
        var lines = File.ReadAllLines(path);
        var n = lines.Length;
        var stat = new double[n];
        for (var i = 0; i < n; i++)
        {
            stat[i] = ParseToDouble(node.SelectSingleNode($".//td[@data-stat='{lines[i]}']")?.InnerText);
        }

        return stat;
    }

    private static double ParseToDouble(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;
        if (str.StartsWith("."))
            str = str.Trim('.').Insert(2, ".");
        if (str.Equals("1.000"))
            str = "100";
        return double.Parse(str.Replace(".", ","));
    }

    private static string FormingName(string name)
    {
        return name.Replace("'", "");
    }
}