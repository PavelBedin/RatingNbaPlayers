namespace PlayerDatabase;

public record Team(int Id, string Name)
{
    public Team() : this(0, "")
    {

    }
}