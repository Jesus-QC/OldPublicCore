﻿using System;
using System.Threading.Tasks;
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
        Connection = new MySqlConnection("Server=; Port=; Database=: Uid=; Pwd=;");
        Log.Warn("Successfully connected!");
    }

    ~Database()
    {
        Connection.Dispose();
    }

    public void InsertNewPlayer(Player player)
    {
        string query = "INSERT INTO NewPlayers (SteamId, DiscordId, NorthWoodId, Username) VALUES ";
        
        query += player.AuthenticationType switch
        {
            AuthenticationType.Steam => $"('{player.RawUserId}', NULL, NULL, '{player.Nickname.Replace("\\", "\\\\").Replace("'", "\\'")}');",
            AuthenticationType.Discord => $"(NULL, '{player.RawUserId}', NULL, '{player.Nickname.Replace("\\", "\\\\").Replace("'", "\\'")}');",
            _ => $"(NULL, NULL, '{player.UserId}', '{player.Nickname.Replace("\\", "\\\\").Replace("'", "\\'")}');"
        };

        ExecuteNonQuery(query);

        int id = player.GetId();
        ExecuteNonQuery($"INSERT INTO Leveling (PlayerId, Exp, Achievements) VALUES ('{id}', 0, '');");
        ExecuteNonQuery($"INSERT INTO SlStats VALUES ('{id}', 0, 0, {(player.DoNotTrack ? 0 : DateTime.UtcNow.Ticks)}, {(player.DoNotTrack ? 0 : DateTime.UtcNow.Ticks)}, 0, 0, 0, 0, 0, 0, 0);");
        ExecuteNonQuery($"INSERT INTO Inventory (PlayerId) VALUES ('{id}')");
    }

    public bool PlayerExists(Player player)
    {
        using MySqlConnection con = Connection.Clone();
        con.Open();
        using MySqlCommand cmd = new ($"SELECT EXISTS(SELECT Id FROM NewPlayers WHERE {player.GetQuery()});", con);
        int exists = (int)(cmd.ExecuteScalar() ?? 0);
        return exists != 0;
    }

    public void ExecuteNonQuery(string command)
    {
        try
        {
            using MySqlConnection con = Connection.Clone();
            con.Open();
            using MySqlCommand cmd = new(command, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Log.Error($"There was an issue executing the query: {command}");
            Log.Warn(e);
        }
    }
    
    public async Task ExecuteNonQueryAsync(string command)
    {
        try
        {
            MySqlConnection con = Connection.Clone();
            await con.OpenAsync(); 
            
            MySqlCommand cmd = new(command, con);
            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await con.DisposeAsync();
        }
        catch (Exception e)
        {
            Log.Error($"There was an issue executing the query: {command}");
            Log.Warn(e);
        }
    }

    public object ExecuteScalar(string command)
    {
        try
        {
            using MySqlConnection con = Connection.Clone();
            con.Open();
            using MySqlCommand cmd = new(command, con);
            return cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            Log.Error($"There was an issue executing the scalar: {command}");
            Log.Warn(e);
            return null;
        }
    }
}
