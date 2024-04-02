namespace DataBase;

public record Player(int Id, string Name)
{
    public Player() : this(0, "")
    {
    }
}