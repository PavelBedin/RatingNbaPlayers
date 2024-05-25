using System.Text;
using PlayerDatabase;

//Spearman's formula is used
class Program
{
    public static void Main()
    {
        var db = new Database();
        var cr = db.GetAllComparisonOfRating();
        var sumOfRatingDif = cr
            .Select(x => Math.Pow(x.Rating2K - ChangeRange(x.RatingBuilt), 2))
            .Sum();
        var t1 = CountT(cr.Select(x => x.RatingBuilt));
        var t2 = CountT(cr.Select(x => x.Rating2K));
        var n = cr.Count();
        var result = 1 - 6 * sumOfRatingDif / (Math.Pow(n, 3) - n - t1 - t2);
        Console.WriteLine(result);
    }

    private static int CountT(IEnumerable<int> data)
    {
        var duplicates = data
            .GroupBy(n => n)
            .Where(g => g.Count() > 1)
            .Select(g => g.Count());
        var result = duplicates.Sum(duplicate => (int)Math.Pow(duplicate, 3) - duplicate);
        return result / 2;
    }

    private static int ChangeRange(int number)
    {
        return (int)Math.Round(70 + number * 0.3);
    }
}