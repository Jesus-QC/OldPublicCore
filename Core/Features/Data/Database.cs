using System;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using MySqlConnector;

namespace Core.Features.Data;

public class Database
{
    public readonly MySqlConnection Connection;

    public Database()
    {
        Log.Warn("Instantiating connection to database...");
        Connection = new MySqlConnection("Server=213.133.102.104; Port=3306; Database=s1_scpsl; Uid=u1_cPwRAvh4eW; Pwd=kP7u.Lhrvh=OaLIqrQQBsJ3k;");
        Log.Warn("Successfully connected!");
    }

    ~Database()
    {
        Connection.Dispose();
    }

    public void InsertNewPlayer(Player player)
    {
        var query = "INSERT INTO NewPlayers (SteamId, DiscordId, NorthWoodId, Username, SubscriptionType) VALUES ";
        switch (player.AuthenticationType)
        {
            case AuthenticationType.Steam:
                query += $"('{player.RawUserId}', NULL, NULL, '{player.Nickname}', '0');";
                break;
            case AuthenticationType.Discord:
                query += $"(NULL, '{player.RawUserId}', NULL, '{player.Nickname}', '0');";
                break;
            default:
                query += $"(NULL, NULL, '{player.UserId}', '{player.Nickname}', '0');";
                break;
        }

        ExecuteNonQuery(query);

        var id = player.GetId();
        ExecuteNonQuery($"INSERT INTO Leveling (PlayerId, Exp, Achievements) VALUES ('{id}', 0, '[0]');");
        ExecuteNonQuery($"INSERT INTO SlStats (PlayerId, RoundsPlayed, TimePlayed, LastSeen) VALUES ('{id}', 0, 0, '{DateTime.UtcNow.Ticks}');");
    }

    public bool PlayerExists(Player player)
    {
        using var con = Connection.Clone();
        con.Open();
        using var cmd = new MySqlCommand($"SELECT EXISTS(SELECT Id FROM NewPlayers WHERE {player.GetQuery()});", con);
        var exists = (int)(cmd.ExecuteScalar() ?? 0);
        return exists != 0;
    }

    public void ExecuteNonQuery(string command)
    {
        try
        {
            using var con = Connection.Clone();
            con.Open();
            using var cmd = new MySqlCommand(command, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Log.Error($"There was an issue executing the query: {command}");
            Log.Warn(e);
        }
    }

    public object ExecuteScalar(string command)
    {
        using var con = Connection.Clone();
        con.Open();
        using var cmd = new MySqlCommand(command, con);
        var obj = cmd.ExecuteScalar();
        return obj;
    }
}