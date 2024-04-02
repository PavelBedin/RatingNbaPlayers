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
                
                db.AddTraditionalStatistics(new TraditionalStatistics(id, FormingTradStat(node)));
            }
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
        }
    }

    private static double[] FormingTradStat(HtmlNode node)
    {
        var playerStats = node
            .SelectNodes(
                ".//td[@data-stat!='player' and @data-stat!='pos' and @data-stat!='team_id' and @data-stat!='age']")
            ?.Select(td => string.IsNullOrEmpty(td.InnerText) ? "0" : td.InnerText.Replace(".", ","))
            .Select(Double.Parse)
            .ToArray();
    }
}