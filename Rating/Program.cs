using PlayerDatabase;
using Rating;

class Program
{
    public static void Main()
    {
        var db = new Database();
        var pcaWorker = new PCAWorker();
        var data = pcaWorker.MakePCA(db.GetAllTraditionalStatistics(), db.GetAllAdvancedStatistics());
    }
}