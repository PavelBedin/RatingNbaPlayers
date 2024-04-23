namespace PlayerDatabase;

public record PlayerInTeam(string NamePlayer, string NameTeam)
{
    public PlayerInTeam() : this("", "")
    {
    }
}