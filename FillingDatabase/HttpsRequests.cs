using HtmlAgilityPack;

class HttpRequests
{
    public async void Main()
    {
        const string url = "https://www.basketball-reference.com/leagues/NBA_2024_per_game.html";

        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var playerNameNodes = doc.DocumentNode.SelectNodes("//td[@data-stat='player']");
                if (playerNameNodes != null)
                {
                    foreach (var node in playerNameNodes)
                    {
                        var playerName = node.InnerText;
                        Console.WriteLine(playerName);
                    }
                }
                else
                {
                    Console.WriteLine("Информация о текущих играющих игроках не найдена.");
                }
            }
            else
            {
                Console.WriteLine($"Ошибка: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}