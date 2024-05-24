using PlayerDatabase;

class Program
{
    public static void Main()
    {
        var db = new Database();
        var cr = db.GetAllComparisonOfRating();
        Console.WriteLine(string.Join(' ', cr));
    }
}