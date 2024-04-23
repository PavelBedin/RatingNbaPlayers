namespace PlayerDatabase;

public interface IStatisticsPlayer
{
    int PlayerId { get; }
    public List<string?> ToList();
    public static abstract IStatisticsPlayer Create(int id, double[] stat);
}