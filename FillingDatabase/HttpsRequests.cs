using HtmlAgilityPack;

internal static class HttpsRequests
{
    public static async Task<HtmlNodeCollection?> GetTraditionalStatistics()
    {
        const string url = "https://www.basketball-reference.com/leagues/NBA_2024_per_game.html";

        using var client = new HttpClient();

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;
        var html = await response.Content.ReadAsStringAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);


        var playerNodes =
            doc.DocumentNode.SelectNodes("//tr[@class='full_table' or @class='italic_text partial_table']");

        return playerNodes ?? playerNodes;
    }
}