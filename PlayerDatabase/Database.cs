using System.Text;
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
        using var insertCommand = _connection.CreateCommand();
        var id = TakeLastId($"{table}") + 1;
        insertCommand.CommandText = $"INSERT INTO {table} (Id, Name) VALUES ({id}, '{name}')";
        insertCommand.ExecuteNonQuery();
        return id;
    }

    public void AddPlayerRating(string namePlayer, int rating, int id = 0)
    {
        if (id == 0)
            id = TakePlayerOrTeamId(namePlayer, "Players");
        if (id == 0)
            id = AddPlayer(namePlayer);
        using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO Rating(PlayerId, Rating) VALUES ({id}, {rating})";
        command.ExecuteNonQuery();
    }

    public void AddPlayerInTeam(string namePlayer, string nameTeam)
    {
        var playerId = 0;
        var teamId = 0;
        if (TakePlayerOrTeamId(namePlayer, "Players") == 0)
            playerId = AddPlayer(namePlayer);
        if (TakePlayerOrTeamId(nameTeam, "Teams") == 0)
            teamId = AddTeam(nameTeam);
        using var command = _connection.CreateCommand();
        command.CommandText = $"INSERT INTO Player_In_Team(PlayerId, TeamId) VALUES ({playerId}, {teamId})";
        command.ExecuteNonQuery();
    }

    public void AddTraditionalStatistics(TraditionalStatistics ts)
    {
        try
        {
            using var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Traditional_Statistics (PlayerId, GamePlayed, MinutesPlayed, ");
            queryBuilder.Append("PPG, FGM, FGA, FGP, TPM, TPA, TPP, FTM, FTA, FTP, OREB, DRED, REB, AST, TOV, ");
            queryBuilder.Append("STL, BLK, PF, DD2, TD3, PM) VALUES (");
            queryBuilder.Append($"{ts.PlayerId}, ");
            queryBuilder.Append($"{ts.GamePlayed}, ");
            queryBuilder.Append($"{ts.MinutesPlayed}, ");
            queryBuilder.Append($"{ts.PPG}, ");
            queryBuilder.Append($"{ts.FGM}, ");
            queryBuilder.Append($"{ts.FGA}, ");
            queryBuilder.Append($"{ts.FGP}, ");
            queryBuilder.Append($"{ts.TPM}, ");
            queryBuilder.Append($"{ts.TPA}, ");
            queryBuilder.Append($"{ts.TPP}, ");
            queryBuilder.Append($"{ts.FTM}, ");
            queryBuilder.Append($"{ts.FTA}, ");
            queryBuilder.Append($"{ts.FTP}, ");
            queryBuilder.Append($"{ts.OREB}, ");
            queryBuilder.Append($"{ts.DRED}, ");
            queryBuilder.Append($"{ts.REB}, ");
            queryBuilder.Append($"{ts.AST}, ");
            queryBuilder.Append($"{ts.TOV}, ");
            queryBuilder.Append($"{ts.STL}, ");
            queryBuilder.Append($"{ts.BLK}, ");
            queryBuilder.Append($"{ts.PF}, ");
            queryBuilder.Append($"{ts.DD2}, ");
            queryBuilder.Append($"{ts.TD3}, ");
            queryBuilder.Append($"{ts.PM})");
            command.CommandText = queryBuilder.ToString();
            command.ExecuteScalar();
        }
        catch (SqliteException e)
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
        return GetAll<PlayerInTeam>("SELECT p.Name AS PlayerId, t.Name FROM Player_In_Team pt " +
                                    "JOIN Players p on p.Id = pt.PlayerId JOIN Teams t on pt.TeamId = t.Id");
    }

    public IEnumerable<PlayerRating?> GetAllPlayerRating()
    {
        return GetAll<PlayerRating>("SELECT Name, Id, Rating FROM Players p JOIN Rating r ON r.PlayerId = p.Id");
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
                id = TakePlayerOrTeamId(name, "Players");
            if (id == 0)
                throw new NotEntryException();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Rating WHERE PlayerId = {id}";
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


    public PlayerInTeam? GetTeamByPlayer(string namePlayer)
    {
        try
        {
            var id = GetPlayerByName(namePlayer).Id;
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT t.Name FROM Player_In_Team pt JOIN Players p on p.Id = pt.PlayerId" +
                                  $" JOIN Teams t on pt.TeamId = t.Id WHERE p.Id = {id}";
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                throw new NotEntryException();
            var nameTeam = reader.GetString(0);
            return new PlayerInTeam(namePlayer, nameTeam);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public IEnumerable<PlayerInTeam>? GetAllPlayerSameTeam(string nameTeam)
    {
        var list = new List<PlayerInTeam>();
        try
        {
            var id = GetTeamByName(nameTeam).Id;
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT p.Name FROM Player_In_Team pt JOIN Players p on p.Id = pt.PlayerId " +
                                  $"JOIN Teams t on pt.TeamId = t.Id WHERE t.Id = {id}";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var namePlayer = reader.GetString(0);
                list.Add(new PlayerInTeam(namePlayer, nameTeam));
            }

            return list;
        }
        catch (Exception e)
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

            list.Add(instance);
        }

        return list;
    }

    private T? GetEntry<T>()
    {
        return default!;
    }

    // public TraditionalStatistics? GetTraditionalStatistics(string name)
    // {
    //     try
    //     {
    //         var id = TakePlayerOrTeamId(name, "Players");
    //         if (id == 0)
    //             throw new NotEntryException();
    //         using var command = _connection.CreateCommand();
    //         command.CommandText = $"SELECT * FROM Traditional_Statistics WHERE Player_Id = {id}";
    //         using var reader = command.ExecuteReader();
    //         if (!reader.Read())
    //             throw new NotEntryException();
    //         var values = new object[24];
    //         reader.GetValues(values);
    //         return new TraditionalStatistics(values);
    //     }
    //     catch (NotEntryException e)
    //     {
    //         Console.WriteLine(e);
    //         return null;
    //     }
    // }

    private int TakeLastId(string tableName)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT MAX(Id) FROM {tableName}";

        return command.ExecuteScalar() == DBNull.Value ? 0 : Convert.ToInt32(command.ExecuteScalar());
    }

    private int TakePlayerOrTeamId(string name, string table)
    {
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT Id FROM {table} WHERE Name = @Name";
        command.Parameters.AddWithValue("@Name", name);
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