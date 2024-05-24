namespace PlayerDatabase;

public record ComparisonOfRatings(string Name, int RatingBuilt, int Rating2K)
{
    public ComparisonOfRatings() : this("", 0, 0)
    {
    }
}