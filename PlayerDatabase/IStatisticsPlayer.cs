namespace DataBase;

public interface IStatisticsPlayer
{
    public List<string?> ToList();
    public static abstract IStatisticsPlayer Create(int id, double[] stat);
}