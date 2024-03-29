namespace Data_base;

public record Player(int Id, string Name)
{
    public Player() : this(0, "")
    {
    }
}