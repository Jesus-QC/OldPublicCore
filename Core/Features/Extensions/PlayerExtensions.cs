using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Features.Components;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Object = UnityEngine.Object;

namespace Core.Features.Extensions;

public static class PlayerExtensions
{
    private static readonly Dictionary<Player, CorePlayerManager> Hubs = new();
    private static readonly Dictionary<Player, int> Ids = new();

    public static void ClearHubs()
    {
        Hubs.Clear();
        Ids.Clear();
    }

    public static void AddToTheHub(this Player player) => Hubs.Add(player, player.GameObject.AddComponent<CorePlayerManager>());
    public static void RemoveFromTheHub(this Player player) => Hubs.Remove(player);

    public static bool Exists(this Player player) => Core.Database.PlayerExists(player);
    public static void AddToTheDatabase(this Player player) => Core.Database.InsertNewPlayer(player);
    public static async Task Authenticate(this Player player)
    {
        try
        {
            if(!player.Exists())
                player.AddToTheDatabase();

            int id = player.GetId();
                
            await Core.Database.ExecuteNonQueryAsync($"UPDATE NewPlayers SET Username='{player.Nickname.Replace("'", "\\'")}' WHERE Id='{id}';");
        }
        catch (Exception e)
        {
            Log.Error(e);
            Log.Warn($"Suppressed authentication error for player {player.Nickname}.");
        }
    }
    public static async void Goodbye(this Player player)
    {
        int secs = 0;

        if (Hubs.ContainsKey(player))
        {
            CorePlayerManager hub = Hubs[player];
            secs = hub.GetSeconds;
            Object.Destroy(Hubs[player]);
            player.RemoveFromTheHub();
        }
            
        if(player.DoNotTrack)
            return;
        
        int id = player.GetId();
        await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET RoundsPlayed=RoundsPlayed+1, TimePlayed=TimePlayed+{secs}, LastSeen='{DateTime.UtcNow.Ticks}' WHERE PlayerId='{id}';");
    }

    public static string GetQuery(this Player player)
    {
        switch (player.AuthenticationType)
        {
            case AuthenticationType.Steam:
                return $"SteamId='{player.RawUserId}'";
            case AuthenticationType.Discord:
                return $"DiscordId='{player.RawUserId}'";
            case AuthenticationType.Northwood:
            case AuthenticationType.Patreon:
                return $"NorthWoodId='{player.UserId}'";
            default:
                return "Id='0'";
        }
    }

    public static int GetId(this Player player)
    {
        if (Ids.ContainsKey(player))
            return Ids[player];

        int id = (int)Core.Database.ExecuteScalar($"SELECT Id FROM NewPlayers WHERE {player.GetQuery()};");
        Ids.Add(player, id);
        return id;
    }

    public static void SendHint(this Player player, ScreenZone zone, string message, float duration = 10)
    {
        if(Hubs.ContainsKey(player))
            Hubs[player].AddMessage(zone, message, duration);
    }

    public static void ClearHint(this Player player, ScreenZone zone)
    {
        if (Hubs.ContainsKey(player))
            Hubs[player].ClearZone(zone);
    }

    public static void SetDisplaySubclass(this Player player, Subclass subclass)
    {
        if (Hubs.ContainsKey(player))
            Hubs[player].SetSubclass(subclass);
    }
}