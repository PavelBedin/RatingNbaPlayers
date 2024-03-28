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
        command.CommandText = $"INSERT INTO Rating(Player_Id, Rating) VALUES ({id}, {rating})";
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
        command.CommandText = $"INSERT INTO Player_In_Team(Player_Id, Team_Id) VALUES ({playerId}, {teamId})";
        command.ExecuteNonQuery();
    }

    public void AddTraditionalStatistics(TraditionalStatistics ts)
    {
        try
        {
            using var command = _connection.CreateCommand();
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Traditional_Statistics (Player_Id, Game_Played, Minutes_Played, ");
            queryBuilder.Append("PPG, FGM, FGA, FGP, TPM, TPA, TPP, FTM, FTA, FTP, OREB, DRED, REB, AST, TOV, ");
            queryBuilder.Append("STL, BLK, PF, DD2, TD3, PM) VALUES (");
            queryBuilder.Append($"{ts.PlayerId}, ");
            queryBuilder.Append($"{ts.GamesPlayed}, ");
            queryBuilder.Append($"{ts.MinutesPlayed}, ");
            queryBuilder.Append($"{ts.PointsPerGame}, ");
            queryBuilder.Append($"{ts.FieldGoalsMade}, ");
            queryBuilder.Append($"{ts.FieldGoalsAttempted}, ");
            queryBuilder.Append($"{ts.FieldGoalsPercentage}, ");
            queryBuilder.Append($"{ts.ThreePointFieldGoalsMade}, ");
            queryBuilder.Append($"{ts.ThreePointFieldGoalsAttempted}, ");
            queryBuilder.Append($"{ts.ThreePointFieldGoalsPercentage}, ");
            queryBuilder.Append($"{ts.FreeThrowsMade}, ");
            queryBuilder.Append($"{ts.FreeThrowsAttempted}, ");
            queryBuilder.Append($"{ts.FreeThrowsPercentage}, ");
            queryBuilder.Append($"{ts.OffensiveRebounds}, ");
            queryBuilder.Append($"{ts.DefensiveRebounds}, ");
            queryBuilder.Append($"{ts.TotalRebounds}, ");
            queryBuilder.Append($"{ts.Assists}, ");
            queryBuilder.Append($"{ts.Turnovers}, ");
            queryBuilder.Append($"{ts.Steals}, ");
            queryBuilder.Append($"{ts.Blocks}, ");
            queryBuilder.Append($"{ts.PersonalFouls}, ");
            queryBuilder.Append($"{ts.DoubleDoubles}, ");
            queryBuilder.Append($"{ts.TripleDoubles}, ");
            queryBuilder.Append($"{ts.PlusMinus})");
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
                id = TakePlayerOrTeamId(name, "Players");
            if (id == 0)
                throw new NotEntryException();
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Rating WHERE Player_Id = {id}";
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

    public IEnumerable<PlayerInTeam> GetAllPlayerInTeams()
    {
        var list = new List<PlayerInTeam>();
        using var command = _connection.CreateCommand();
        command.CommandText = "SELECT p.Name, t.Name FROM Player_In_Team pt " +
                              "JOIN Players p on p.Id = pt.Player_Id JOIN Teams t on pt.Team_Id = t.Id";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var namePlayer = reader.GetString(0);
            var nameTeam = reader.GetString(1);
            list.Add(new PlayerInTeam(namePlayer, nameTeam));
        }

        return list;
    }

    public PlayerInTeam? GetTeamByPlayer(string namePlayer)
    {
        try
        {
            var id = GetPlayerByName(namePlayer).Id;
            using var command = _connection.CreateCommand();
            command.CommandText = $"SELECT t.Name FROM Player_In_Team pt JOIN Players p on p.Id = pt.Player_Id" +
                                  $" JOIN Teams t on pt.Team_Id = t.Id WHERE p.Id = {id}";
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
            command.CommandText = $"SELECT p.Name FROM Player_In_Team pt JOIN Players p on p.Id = pt.Player_Id " +
                                  $"JOIN Teams t on pt.Team_Id = t.Id WHERE t.Id = {id}";
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


    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}