namespace PlayerDatabase;

public record PlayerRating(string Name, int Id, int Rating)
{
    public PlayerRating() : this("", 0, 0)
    {}
}

