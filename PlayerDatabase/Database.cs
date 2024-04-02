using System.Reflection;
using System.Text;
using Data_base.Exceptions;
using Microsoft.Data.Sqlite;

namespace DataBase;

public class Database : IDisposable
{
    private readonly SqliteConnection _connection;

    public Database(string dataBaseName = "NBA.db")
    {
        _connection = new SqliteConnection($"Data Source={dataBaseName}");
        _connection.Open();
    }


    public int AddPlayer(string namePlayer)
    {
        return AddPlayerOrTeam(namePlayer, "Players");
    }

    public int AddTeam(string nameTeam)
    {
        return AddPlayerOrTeam(nameTeam, "Teams");
    }

    private int AddPlayerOrTeam(string name, string table)
    {
        var id = TakeLastId($"{table}") + 1;
        AddEntry(table, $"{id}, '{name}'");
        return id;
    }

    public void AddPlayerRating(PlayerRating rating)
    {
        var id = rating.Id;
        if (id == 0)
            id = TakePlayerOrTeamId(rating.Name, "Players");
        if (id == 0)
            id = AddPlayer(rating.Name);
        AddEntry("Rating", $"{id}, {rating.Rating}");
    }

    public void AddPlayerInTeam(string namePlayer, string nameTeam)
    {
        var playerId = 0;
        var teamId = 0;
        if (TakePlayerOrTeamId(namePlayer, "Players") == 0)
            playerId = AddPlayer(namePlayer);
        if (TakePlayerOrTeamId(nameTeam, "Teams") == 0)
            teamId = AddTeam(nameTeam);
        AddEntry("Player_In_Team", $"{playerId}, {teamId}");
    }

    public void AddTraditionalStatistics(TraditionalStatistics ts)
    {
        try
        {
            if (ts.PlayerId == 0)
                throw new NotEntryException();
            if (GetPlayerById(ts.PlayerId) == null)
                throw new NotEntryException();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"{ts.PlayerId}, {ts.GamePlayed}, {ts.MinutesPlayed}, {ts.PPG}, {ts.FGM}, {ts.FGA}, ");
            queryBuilder.Append($"{ts.FGP}, {ts.TPM}, {ts.TPA}, {ts.TPP}, {ts.FTM}, {ts.FTA}, {ts.FTP}, {ts.OREB}, ");
            queryBuilder.Append($"{ts.DRED}, {ts.REB}, {ts.AST}, {ts.TOV}, {ts.STL}, {ts.BLK}, {ts.PF}, {ts.DD2}, ");
            queryBuilder.Append($"{ts.TD3}, {ts.PM}");
            AddEntry("Traditional_Statistics", queryBuilder.ToString());
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
        }
    }

    public void AddAdvancedStatistics(AdvancedStatistics ads)
    {
        try
        {
            if (ads.PlayerId == 0)
                throw new NotEntryException();
            if (GetPlayerById(ads.PlayerId) == null)
                throw new NotEntryException();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"{ads.PlayerId}, {ads.OFFRTG}, {ads.DEFRTG}, {ads.NETRTG}, {ads.ASTP}, {ads.ASTTO}, ");
            queryBuilder.Append($"{ads.ASTR}, {ads.OREBP}, {ads.DREBP}, {ads.REBP}, {ads.TOR}, {ads.EFGP}, ");
            queryBuilder.Append($"{ads.TSP}, {ads.USGP}, {ads.PACE}, {ads.PIE}");
            AddEntry("Advanced_Statistics", queryBuilder.ToString());
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
        }
    }

    public IEnumerable<Player?> GetAllPlayers()
    {
        return GetAll<Player>(DefaultSelect("Players"));
    }

    public IEnumerable<Team?> GetAllTeams()
    {
        return GetAll<Team>(DefaultSelect("Teams"));
    }

    public IEnumerable<TraditionalStatistics?> GetAllTraditionalStatistics()
    {
        return GetAll<TraditionalStatistics>(DefaultSelect("Traditional_Statistics"));
    }

    public IEnumerable<PlayerInTeam?> GetAllPlayerInTeams()
    {
        return GetAll<PlayerInTeam>("SELECT p.Name AS NamePlayer, t.Name AS NameTeam FROM Player_In_Team pt " +
                                    "JOIN Players p on p.Id = pt.PlayerId JOIN Teams t on pt.TeamId = t.Id");
    }

    public IEnumerable<PlayerRating?> GetAllPlayerRating()
    {
        return GetAll<PlayerRating>("SELECT Name, Id, Rating FROM Players p JOIN Rating r ON r.PlayerId = p.Id");
    }

    public IEnumerable<PlayerInTeam>? GetAllPlayerSameTeam(string nameTeam)
    {
        try
        {
            var id = GetTeamByName(nameTeam).Id;
            return GetAll<PlayerInTeam>(
                $"SELECT p.Name AS NamePlayer, t.Name AS NameTeam FROM Player_In_Team pt" +
                $" JOIN Players p on p.Id = pt.PlayerId JOIN Teams t on pt.TeamId = t.Id WHERE t.Id = {id}")!;
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public IEnumerable<AdvancedStatistics?> GetAllAdvancedStatistics()
    {
        return GetAll<AdvancedStatistics>(DefaultSelect("Advanced_Statistics"));
    }


    public Player? GetPlayerById(int id)
    {
        return TryToGetEntry<Player>($"SELECT * FROM Players WHERE Id = {id}");
    }


    public Player? GetPlayerByName(string name)
    {
        return TryToGetEntry<Player>($"SELECT * FROM Players WHERE Name = '{name}'");
    }


    public Team? GetTeamById(int id)
    {
        return TryToGetEntry<Team?>($"SELECT * FROM Teams WHERE Id = {id}");
    }

    public Team? GetTeamByName(string name)
    {
        return TryToGetEntry<Team?>($"SELECT * FROM Teams WHERE Name = '{name}'");
    }

    public PlayerInTeam? GetTeamByPlayer(string namePlayer)
    {
        try
        {
            var id = GetPlayerByName(namePlayer).Id;
            return TryToGetEntry<PlayerInTeam>(
                $"SELECT p.Name AS NamePlayer, t.Name AS NameTeam FROM Player_In_Team pt JOIN Players p on p.Id = pt.PlayerId" +
                $" JOIN Teams t on pt.TeamId = t.Id WHERE p.Id = {id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public PlayerRating? GetPlayerRating(string name, int id = 0)
    {
        try
        {
            if (id == 0)
                id = GetPlayerByName(name).Id;
            return TryToGetEntry<PlayerRating>(
                $"SELECT Name, Id, Rating FROM Rating r JOIN Players p on p.Id = r.PlayerId WHERE PlayerId = {id}");
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public TraditionalStatistics? GetTraditionalStatistics(string name, int id = 0)
    {
        try
        {
            if (id == 0)
                id = GetPlayerByName(name).Id;
            return TryToGetEntry<TraditionalStatistics>(
                $"SELECT * FROM Traditional_Statistics WHERE PlayerId = {id}");
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public AdvancedStatistics? GetAdvancedStatistics(string name, int id = 0)
    {
        try
        {
            if (id == 0)
                id = GetPlayerByName(name).Id;
            return TryToGetEntry<AdvancedStatistics>(
                $"SELECT * FROM Advanced_Statistics WHERE PlayerId = {id}");
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private IEnumerable<T?> GetAll<T>(string select)
    {
        var list = new List<T?>();
        using var command = _connection.CreateCommand();
        command.CommandText = select;
        using var reader = command.ExecuteReader();
        var properties = typeof(T).GetProperties();
        var fieldCount = reader.FieldCount;
        while (reader.Read())
        {
            list.Add(GetEntry<T>(reader, properties, fieldCount));
        }

        return list;
    }

    private T GetEntry<T>(SqliteDataReader reader, PropertyInfo[] properties, int fieldCount)
    {
        var instance = Activator.CreateInstance<T>();
        for (var i = 0; i < fieldCount; i++)
        {
            var fieldName = reader.GetName(i);
            var property =
                properties.FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (property == null || reader.IsDBNull(i)) continue;
            var value = reader.GetValue(i);
            if (value != DBNull.Value)
                property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
        }

        return instance;
    }

    private T? TryToGetEntry<T>(string select)
    {
        try
        {
            using var command = _connection.CreateCommand();
            command.CommandText = select;
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                throw new NotEntryException();
            var properties = typeof(T).GetProperties();
            var fieldCount = reader.FieldCount;
            return GetEntry<T>(reader, properties, fieldCount);
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return default!;
        }
    }

    private void AddEntry(string table, string values)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO {table} VALUES ({values})";
        command.ExecuteNonQuery();
    }

    private int TakeLastId(string tableName)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT MAX(Id) FROM {tableName}";

        return command.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(command.ExecuteScalar());
    }

    private int TakePlayerOrTeamId(string name, string table)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT Id FROM {table} WHERE Name = '{name}'";
        return command.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(command.ExecuteScalar());
    }

    private string DefaultSelect(string table)
    {
        return $"SELECT * FROM {table}";
    }


    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}