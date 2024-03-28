using Data_base;
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


    public void AddPlayer(string namePlayer)
    {
        AddPlayerOrTeam(namePlayer, "Players");
    }

    public void AddTeam(string nameTeam)
    {
        AddPlayerOrTeam(nameTeam, "Teams");
    }

    private void AddPlayerOrTeam(string name, string table)
    {
        using var insertCommand = _connection.CreateCommand();
        var id = TakeLastId($"{table}") + 1;
        insertCommand.CommandText = $"INSERT INTO {table} (Id, Name) VALUES ({id}, '{name}')";
        insertCommand.ExecuteNonQuery();
    }

    public void AddPlayerRating(string namePlayer, int rating, int id = 0)
    {
        if (id == 0)
            id = TakePlayerId(namePlayer);
        if (id == 0)
        {
            AddPlayer(namePlayer);
            id = TakeLastId("Players");
        }

        using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO Rating(Player_Id, Rating) VALUES ({id}, {rating})";
        command.ExecuteNonQuery();
    }

    public IEnumerable<Player?> GetAllPlayers()
    {
        return GetAllFromPlayersOrTeams<Player>("Players");
    }

    public IEnumerable<Team?> GetAllTeams()
    {
        return GetAllFromPlayersOrTeams<Team>("Teams");
    }

    private IEnumerable<T?> GetAllFromPlayersOrTeams<T>(string table)
    {
        var list = new List<T?>();
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {table}";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            list.Add((T)Activator.CreateInstance(typeof(T), id, name)!);
        }

        return list;
    }


    public Player GetPlayerById(int id)
    {
        return GetEntryPlayerOrTeam<Player>(table: "Players", id: id);
    }


    public Player GetPlayerByName(string name)
    {
        return GetEntryPlayerOrTeam<Player>(table: "Players", name: name);
    }


    public Team GetTeamById(int id)
    {
        return GetEntryPlayerOrTeam<Team>(id: id, table: "Teams");
    }

    public Team GetTeamByName(string name)
    {
        return GetEntryPlayerOrTeam<Team>(name: name, table: "Teams");
    }

    private T GetEntryPlayerOrTeam<T>(string table, int id = 0, string name = "")
    {
        using var command = _connection.CreateCommand();
        if (id != 0)
        {
            command.CommandText = $"SELECT * FROM {table} WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
        }
        else
        {
            command.CommandText = $"SELECT * FROM {table} WHERE Name = @Name";
            command.Parameters.AddWithValue("@Name", name);
        }

        using var reader = command.ExecuteReader();
        try
        {
            if (!reader.Read())
                throw new NotEntryException();
            id = reader.GetInt32(0);
            name = reader.GetString(1);
            return (T)Activator.CreateInstance(typeof(T), id, name)!;
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return default!;
        }
    }

    public PlayerRating? GetPlayerRating(string name, int id = 0)
    {
        try
        {
            if (id == 0)
                id = TakePlayerId(name);
            if (id == 0)
                throw new NotEntryException();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Rating WHERE Id = {id}";
            using var reader = command.ExecuteReader();
            var rating = 0;
            if (reader.Read())
                rating = reader.GetInt32(1);
            return rating == 0 ? null : new PlayerRating(name, id, rating);
        }
        catch (NotEntryException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public IEnumerable<PlayerRating> GetAllPlayerRating()
    {
        var list = new List<PlayerRating>();
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT Name, Id, Rating FROM Players p JOIN Rating r ON r.Player_Id = p.Id";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var name = reader.GetString(0);
            var id = reader.GetInt32(1);
            var rating = reader.GetInt32(2);
            list.Add(new PlayerRating(name, id, rating));
        }

        return list;
    }

    private int TakeLastId(string tableName)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT MAX(Id) FROM {tableName}";

        return command.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(command.ExecuteScalar());
    }

    private int TakePlayerId(string name)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT Id FROM Players WHERE Name = @Name";
        command.Parameters.AddWithValue("@Name", name);
        return command.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(command.ExecuteScalar());
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}