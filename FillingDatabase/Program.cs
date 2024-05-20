using HtmlAgilityPack;
using PlayerDatabase;

namespace FillingDatabase;

using AllPaths;

internal delegate void AddStatistics(IStatisticsPlayer statistics);

internal delegate IStatisticsPlayer Create(int id, double[] stat);

public static class Program
{
    private static readonly Database Db = new();
    private static readonly HashSet<string> AllPlayers = new();

    public static void Main()
    {
        var path = new FindPath();
        AddToDatabase(player => Db.AddTraditionalStatistics((TraditionalStatistics)player),
            HttpsRequests.GetTraditionalStatistics(),
            TraditionalStatistics.Create, path.GetFullPath("FieldsTradStat"), true);
        AddToDatabase(player => Db.AddAdvancedStatistics((AdvancedStatistics)player),
            HttpsRequests.GetAdvancedStatistics(),
            AdvancedStatistics.Create, path.GetFullPath("FieldsAdvStat"));
    }

    private static void AddToDatabase(AddStatistics statisticsDelegate, HtmlNodeCollection collection,
        Create createDelegate, string path, bool isFirst = false)
    {
        try
        {
            if (collection == null)
                throw new NullReferenceException();
            var players = new HashSet<string>();
            foreach (var node in collection)
            {
                var playerName = node.SelectSingleNode(".//td[@data-stat='player']")?.InnerText;
                if (playerName == null)
                    continue;
                playerName = FormingName(playerName);
                if (players.Contains(playerName))
                    continue;
                players.Add(playerName);
                int id;
                if (isFirst)
                {
                    var innerText = node.SelectSingleNode(".//td[@data-stat='g']")?.InnerText;
                    if (innerText != null && Int32.Parse(innerText) < 55)
                        continue;
                    AllPlayers.Add(playerName);
                    id = Db.AddPlayer(playerName);
                }
                else
                {
                    if (!AllPlayers.Contains(playerName))
                        continue;
                    id = Db.GetPlayerByName(playerName).Id;
                }

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
            if (Math.Abs(stat[i] - 100) < 0.0001 && stat[i - 1] == 0)
                stat[i] = 0;
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