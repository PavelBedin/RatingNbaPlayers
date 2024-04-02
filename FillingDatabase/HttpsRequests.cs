using HtmlAgilityPack;

class HttpsRequests
{
    public static async Task GetTraditionalStatistics()
    {
        const string url = "https://www.basketball-reference.com/leagues/NBA_2024_per_game.html";

        using var client = new HttpClient();

        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var html = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);


            var playerNodes =
                doc.DocumentNode.SelectNodes("//tr[@class='full_table' or @class='italic_text partial_table']");

            if (playerNodes != null)
            {
                foreach (var node in playerNodes)
                {
                    var playerName = node.SelectSingleNode(".//td[@data-stat='player']/a")?.InnerText;

                    var playerStats = node.SelectNodes(".//td[@data-stat!='player']")
                        .Select(td => td.InnerText)
                        .ToList();

                    Console.WriteLine($"Имя: {playerName}");
                    Console.WriteLine("Статистика:");
                    foreach (var stat in playerStats)
                    {
                        Console.WriteLine(stat);
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}