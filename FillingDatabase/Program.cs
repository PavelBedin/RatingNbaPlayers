using HtmlAgilityPack;

namespace FillingDatabase;

using DataBase;

public class Program
{
    public static void Main()
    {
        var db = new Database(File.ReadAllText("path.txt").Trim());
        try
        {
            var collectionTradStat = HttpsRequests.GetTraditionalStatistics().Result;
            if (collectionTradStat == null)
                throw new NullReferenceException();
            var players = new HashSet<string>();
            foreach (var node in collectionTradStat)
            {
                var playerName = node.SelectSingleNode(".//td[@data-stat='player']")?.InnerText;
                if (playerName == null || players.Contains(playerName))
                    continue;
                var id = db.AddPlayer(playerName);
                players.Add(playerName);

                db.AddTraditionalStatistics(new TraditionalStatistics(id, FormingStat(node, "FieldsTradStat.txt")));
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
            str.Trim('.').Insert(1, ".");
        return double.Parse(str.Replace(".", ","));
    }
}